using Microsoft.AspNetCore.Mvc;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services;

namespace RoyalVillaWeb.Controllers;

public class AccountController(IApiClient api) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid) return View(model);
        var (token, error) = await api.LoginAsync(model.Email, model.Password);
        if (token is null)
        {
            ModelState.AddModelError(string.Empty, error ?? "Sign in failed.");
            return View(model);
        }

        HttpContext.Session.SetString("ApiToken", token);
        var isAdmin = AuthState.IsAdmin(token);
        TempData["Success"] =
            isAdmin ? "Welcome back. You can now manage villas." : "You’re signed in. Welcome to Royal Villa.";
        if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl!);
        return isAdmin ? RedirectToAction("Index", "Villas") : RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var (success, error) = await api.RegisterAsync(model);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, error ?? "Your account could not be created.");
            return View(model);
        }

        TempData["Success"] = "Your account is ready. Sign in with your email and password.";
        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("ApiToken");
        TempData["Success"] = "You have been signed out.";
        return RedirectToAction("Index", "Home");
    }
}