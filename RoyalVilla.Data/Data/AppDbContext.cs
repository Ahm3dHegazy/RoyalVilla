using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RoyalVilla_API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Villa>  Villas { get; set; }
    public DbSet<VillaAmenities> VillaAmenities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}