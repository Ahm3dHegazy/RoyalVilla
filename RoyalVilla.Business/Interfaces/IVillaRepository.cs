namespace RoyalVilla_API.Interfaces;

public interface IVillaRepository
{
    Task<IReadOnlyList<Villa>> GetAllAsync();
    Task<Villa?> GetByIdAsync(int id);
    Task<Villa?> GetForUpdateAsync(int id);
    Task<bool> NameExistsAsync(string name, int? exceptId = null);
    Task AddAsync(Villa villa);
    Task<bool> DeleteAsync(int id);
    Task SaveChangesAsync();
}
