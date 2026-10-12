using RoyalVilla_API.Interfaces;

namespace RoyalVilla_API.Services;

public class VillaService(IVillaRepository repository, IMapper mapper) : IVillaService
{
    public async Task<IEnumerable<VillaDto>> GetAllAsync()
        => mapper.Map<IEnumerable<VillaDto>>(await repository.GetAllAsync());

    public async Task<VillaDto?> GetByIdAsync(int id)
    {
        var villa = await repository.GetByIdAsync(id);
        return villa is null ? null : mapper.Map<VillaDto>(villa);
    }

    public async Task<VillaDto?> CreateAsync(CreateVillaDto dto)
    {
        var villa = mapper.Map<Villa>(dto);
        await repository.AddAsync(villa);
        await repository.SaveChangesAsync();
        return mapper.Map<VillaDto>(villa);
    }

    public Task<bool> NameExistsAsync(string name, int? exceptId = null)
        => repository.NameExistsAsync(name, exceptId);

    public async Task<bool> UpdateAsync(int id, UpdateVillaDto dto)
    {
        var villa = await repository.GetForUpdateAsync(id);
        if (villa is null)
            return false;

        mapper.Map(dto, villa);
        await repository.SaveChangesAsync();
        return true;
    }

    public Task<bool> DeleteAsync(int id)
        => repository.DeleteAsync(id);
}
