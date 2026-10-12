using RoyalVilla_API.Interfaces;

namespace RoyalVilla_API.Services;

public class VillaAmenitiesService(IVillaAmenitiesRepository repository, IMapper mapper) : IVillaAmenitiesService
{
    public async Task<IReadOnlyList<VillaAmenitiesDto>?> GetByVillaIdAsync(int villaId)
    {
        if (!await repository.VillaExistsAsync(villaId))
            return null;

        return mapper.Map<IReadOnlyList<VillaAmenitiesDto>>(
            await repository.GetByVillaIdAsync(villaId));
    }

    public async Task<VillaAmenitiesDto?> GetByIdAsync(int id)
    {
        var amenity = await repository.GetByIdAsync(id);
        return amenity is null ? null : mapper.Map<VillaAmenitiesDto>(amenity);
    }

    public Task<bool> VillaExistsAsync(int villaId)
        => repository.VillaExistsAsync(villaId);

    public async Task<VillaAmenitiesDto?> CreateAsync(CreateVillaAmenitiesDto dto)
    {
        if (!await repository.VillaExistsAsync(dto.VillaId))
            return null;

        var amenity = mapper.Map<VillaAmenities>(dto);
        var now = DateTime.UtcNow;
        amenity.CreatedAt = now;
        amenity.UpdatedAt = now;
        await repository.AddAsync(amenity);
        await repository.SaveChangesAsync();

        return await GetByIdAsync(amenity.Id);
    }

    public async Task<bool> UpdateAsync(int id, UpdateVillaAmenitiesDto dto)
    {
        var amenity = await repository.GetForUpdateAsync(id);
        if (amenity is null || !await repository.VillaExistsAsync(dto.VillaId))
            return false;

        mapper.Map(dto, amenity);
        amenity.UpdatedAt = DateTime.UtcNow;
        await repository.SaveChangesAsync();
        return true;
    }

    public Task<bool> DeleteAsync(int id)
        => repository.DeleteAsync(id);
}
