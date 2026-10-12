using RoyalVilla_API.Interfaces;

namespace RoyalVilla_API.Data.Repositories;

public class VillaAmenitiesRepository(AppDbContext context) : IVillaAmenitiesRepository
{
    public Task<bool> VillaExistsAsync(int villaId)
        => context.Villas.AnyAsync(villa => villa.Id == villaId);

    public async Task<IReadOnlyList<VillaAmenities>> GetByVillaIdAsync(int villaId)
        => await context.VillaAmenities.AsNoTracking()
            .Include(amenity => amenity.Villa)
            .Where(amenity => amenity.VillaId == villaId)
            .ToListAsync();

    public Task<VillaAmenities?> GetByIdAsync(int id)
        => context.VillaAmenities.AsNoTracking()
            .Include(amenity => amenity.Villa)
            .FirstOrDefaultAsync(amenity => amenity.Id == id);

    public Task<VillaAmenities?> GetForUpdateAsync(int id)
        => context.VillaAmenities.FirstOrDefaultAsync(amenity => amenity.Id == id);

    public async Task AddAsync(VillaAmenities amenity)
        => await context.VillaAmenities.AddAsync(amenity);

    public async Task<bool> DeleteAsync(int id)
    {
        var amenity = await context.VillaAmenities.FindAsync(id);
        if (amenity is null)
            return false;

        context.VillaAmenities.Remove(amenity);
        await context.SaveChangesAsync();
        return true;
    }

    public Task SaveChangesAsync()
        => context.SaveChangesAsync();
}
