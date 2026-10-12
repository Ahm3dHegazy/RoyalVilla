namespace RoyalVilla_API.Interfaces;

public interface IVillaService
{
    Task<IEnumerable<VillaDto>> GetAllAsync();
    Task<VillaDto?> GetByIdAsync(int id);
    Task<VillaDto?> CreateAsync(CreateVillaDto dto);
    Task<bool> NameExistsAsync(string name, int? exceptId = null);
    Task<bool> UpdateAsync(int id, UpdateVillaDto dto);
    Task<bool> DeleteAsync(int id);
}
