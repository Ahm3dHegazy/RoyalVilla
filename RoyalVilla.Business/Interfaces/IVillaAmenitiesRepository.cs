namespace RoyalVilla_API.Interfaces;

public interface IVillaAmenitiesRepository
{
    Task<bool> VillaExistsAsync(int villaId);
    Task<IReadOnlyList<VillaAmenities>> GetByVillaIdAsync(int villaId);
    Task<VillaAmenities?> GetByIdAsync(int id);
    Task<VillaAmenities?> GetForUpdateAsync(int id);
    Task AddAsync(VillaAmenities amenity);
    Task<bool> DeleteAsync(int id);
    Task SaveChangesAsync();
}
