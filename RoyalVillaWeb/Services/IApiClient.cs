using RoyalVillaWeb.Models;

namespace RoyalVillaWeb.Services;

public interface IApiClient
{
    Task<(IReadOnlyList<VillaDto> Items, string? Error)> GetVillasAsync();
    Task<(VillaDto? Item, string? Error)> GetVillaAsync(int id);
    Task<(IReadOnlyList<AmenityDto> Items, string? Error)> GetAmenitiesAsync(int villaId);
    Task<(bool Success, string? Error)> CreateVillaAsync(VillaFormViewModel villa);
    Task<(bool Success, string? Error)> UpdateVillaAsync(VillaFormViewModel villa);
    Task<(bool Success, string? Error)> DeleteVillaAsync(int id);
    Task<(string? Token, string? Error)> LoginAsync(string email, string password);
    Task<(bool Success, string? Error)> RegisterAsync(RegisterViewModel account);
}
