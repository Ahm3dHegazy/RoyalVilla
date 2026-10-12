using Microsoft.AspNetCore.Mvc;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services;

namespace RoyalVillaWeb.Controllers;

public class VillasController(IApiClient api) : Controller
{
    public async Task<IActionResult> Index(string? q)
    {
        var (items, error) = await api.GetVillasAsync();
        var villas = string.IsNullOrWhiteSpace(q) ? items : items.Where(v =>
            (v.Name?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (v.Details?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)).ToArray();
        return View(new VillasPageViewModel { Villas = villas, Query = q, Error = error, IsAdmin = IsAdmin() });
    }

    public async Task<IActionResult> Details(int id)
    {
        var (villa, error) = await api.GetVillaAsync(id);
        if (villa is null)
        {
            TempData["Error"] = error ?? "We couldn't find that villa.";
            return RedirectToAction(nameof(Index));
        }
        var (amenities, _) = await api.GetAmenitiesAsync(id);
        return View(new VillaDetailsViewModel { Villa = villa, Amenities = amenities, IsAdmin = IsAdmin() });
    }

    [HttpGet]
    public IActionResult Create() => IsAdmin() ? View("Form", new VillaFormViewModel()) : Forbid();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VillaFormViewModel model)
    {
        if (!IsAdmin()) return Forbid();
        if (!ModelState.IsValid) return View("Form", model);
        var (success, error) = await api.CreateVillaAsync(model);
        if (!success) { ModelState.AddModelError(string.Empty, error ?? "Villa could not be created."); return View("Form", model); }
        TempData["Success"] = "Villa created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!IsAdmin()) return Forbid();
        var (villa, error) = await api.GetVillaAsync(id);
        if (villa is null) { TempData["Error"] = error ?? "Villa not found."; return RedirectToAction(nameof(Index)); }
        return View("Form", new VillaFormViewModel { Id = villa.Id, Name = villa.Name ?? "", Details = villa.Details, Rate = villa.Rate, Sqft = villa.Sqft, Occupancy = villa.Occupancy, ImageUrl = villa.ImageUrl });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, VillaFormViewModel model)
    {
        if (!IsAdmin()) return Forbid();
        model.Id = id;
        if (!ModelState.IsValid) return View("Form", model);
        var (success, error) = await api.UpdateVillaAsync(model);
        if (!success) { ModelState.AddModelError(string.Empty, error ?? "Villa could not be updated."); return View("Form", model); }
        TempData["Success"] = "Villa updated successfully.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsAdmin()) return Forbid();
        var (villa, error) = await api.GetVillaAsync(id);
        if (villa is null) { TempData["Error"] = error ?? "Villa not found."; return RedirectToAction(nameof(Index)); }
        return View(villa);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!IsAdmin()) return Forbid();
        var (success, error) = await api.DeleteVillaAsync(id);
        if (!success) { TempData["Error"] = error ?? "Villa could not be deleted."; return RedirectToAction(nameof(Index)); }
        TempData["Success"] = "Villa deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private bool IsAdmin() => AuthState.IsAdmin(HttpContext.Session.GetString("ApiToken"));
}
