using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RoyalVilla_API.Interfaces;

namespace RoyalVilla_API.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration) : IAuthService
{
    private const string DefaultRole = "User";

    public async Task<bool> IsUsernameExistsAsync(string username)
        => await userManager.FindByNameAsync(username) is not null;

    public async Task<bool> IsEmailExistsAsync(string email)
        => await userManager.FindByEmailAsync(email) is not null;
    
    public async Task<IdentityResult> RegisterAsync(RegisterDto model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };
        
         var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return result;

        var roleResult = await userManager.AddToRoleAsync(user, DefaultRole);
        if (!roleResult.Succeeded)
        {
          
            await userManager.DeleteAsync(user);
            return roleResult;
        }

        return IdentityResult.Success;
    }

    public async Task<string?> LoginAsync(LoginDto model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        
        if (user is null || !await userManager.CheckPasswordAsync(user, model.Password))
            return null;

        return await GenerateTokenAsync(user);
    }


    private async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        var jwt = configuration.GetSection("Jwt");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!)
        };

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpiryMinutes"]!)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
