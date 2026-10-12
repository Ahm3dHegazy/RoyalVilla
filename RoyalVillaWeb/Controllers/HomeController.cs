using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services;

namespace RoyalVillaWeb.Controllers;

public class HomeController(IApiClient api) : Controller
{
    public async Task<IActionResult> Index()
    {
        var (villas, error) = await api.GetVillasAsync();
        var model = new VillasPageViewModel { Villas = villas, Error = error, IsAdmin = IsAdmin() };
        return View(model);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });

    private bool IsAdmin() => AuthState.IsAdmin(HttpContext.Session.GetString("ApiToken"));
}

public static class AuthState
{
    public static bool IsAdmin(string? token)
    {
        if (string.IsNullOrWhiteSpace(token)) return false;
        try
        {
            var parts = token.Split('.');
            if (parts.Length < 2) return false;
            var payload = parts[1].Replace('-', '+').Replace('_', '/');
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            using var doc = JsonDocument.Parse(Convert.FromBase64String(payload));
            foreach (var claim in doc.RootElement.EnumerateObject())
            {
                if (claim.Name is "role" or "roles" or "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                {
                    if (claim.Value.ValueKind == JsonValueKind.String && claim.Value.GetString() == "Admin") return true;
                    if (claim.Value.ValueKind == JsonValueKind.Array && claim.Value.EnumerateArray().Any(x => x.GetString() == "Admin")) return true;
                }
            }
        }
        catch (Exception ex) when (ex is FormatException or JsonException) { }
        return false;
    }
}
