namespace RoyalVilla_API.Interfaces;

public interface IVillaAmenitiesService
{
    Task<IReadOnlyList<VillaAmenitiesDto>?> GetByVillaIdAsync(int villaId);
    Task<VillaAmenitiesDto?> GetByIdAsync(int id);
    Task<bool> VillaExistsAsync(int villaId);
    Task<VillaAmenitiesDto?> CreateAsync(CreateVillaAmenitiesDto dto);
    Task<bool> UpdateAsync(int id, UpdateVillaAmenitiesDto dto);
    Task<bool> DeleteAsync(int id);
}
