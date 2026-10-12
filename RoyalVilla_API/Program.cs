using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using RoyalVilla_API.Interfaces;
using RoyalVilla_API.Data.Repositories;
using RoyalVilla_API.Mappings;
using RoyalVilla_API.Services;

namespace RoyalVilla_API;

public abstract class Program
{
    public static async Task Main(string[] args)
    {
        // Create the application builder.
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();
        
        await SeedIdentityAsync(app);
        
        ConfigurePipeline(app);

        await app.RunAsync();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        ConfigureApiDocumentation(builder.Services);
        
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IVillaRepository, VillaRepository>();
        builder.Services.AddScoped<IVillaAmenitiesRepository, VillaAmenitiesRepository>();
        builder.Services.AddScoped<IVillaService, VillaService>();
        builder.Services.AddScoped<IVillaAmenitiesService, VillaAmenitiesService>();
        builder.Services.AddAutoMapper(cfg => { }, typeof(VillaProfile));
        
        builder.Services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
        
        ConfigureAuthentication(builder);
    }

    private static void ConfigureApiDocumentation(IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, _) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };

                foreach (var path in document.Paths.Values)
                {
                    if (path.Operations is null)
                        continue;

                    foreach (var operation in path.Operations.Values)
                    {
                        operation.Security ??= [];
                        operation.Security.Add(new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                        });
                    }
                }

                return Task.CompletedTask;
            });
        });
    }

    private static void ConfigureAuthentication(WebApplicationBuilder builder)
    {
        var jwt = builder.Configuration.GetSection("Jwt");

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwt["Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }

    private static async Task SeedIdentityAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await IdentitySeeder.SeedAsync(scope.ServiceProvider);
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }
        
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}
