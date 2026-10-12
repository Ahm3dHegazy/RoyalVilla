using RoyalVilla_API.Interfaces;

namespace RoyalVilla_API.Data.Repositories;

public class VillaRepository(AppDbContext context) : IVillaRepository
{
    public async Task<IReadOnlyList<Villa>> GetAllAsync()
        => await context.Villas.AsNoTracking().ToListAsync();

    public Task<Villa?> GetByIdAsync(int id)
        => context.Villas.AsNoTracking().FirstOrDefaultAsync(villa => villa.Id == id);

    public Task<Villa?> GetForUpdateAsync(int id)
        => context.Villas.FirstOrDefaultAsync(villa => villa.Id == id);

    public Task<bool> NameExistsAsync(string name, int? exceptId = null)
        => context.Villas.AnyAsync(villa => villa.Name!.ToLower() == name.ToLower()
                                             && (!exceptId.HasValue || villa.Id != exceptId.Value));

    public async Task AddAsync(Villa villa)
        => await context.Villas.AddAsync(villa);

    public async Task<bool> DeleteAsync(int id)
    {
        var villa = await context.Villas.FindAsync(id);
        if (villa is null)
            return false;

        context.Villas.Remove(villa);
        await context.SaveChangesAsync();
        return true;
    }

    public Task SaveChangesAsync()
        => context.SaveChangesAsync();
}
