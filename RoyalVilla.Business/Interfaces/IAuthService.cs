using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using RoyalVilla_API.Services;

namespace RoyalVilla_API.Interfaces;

public interface IAuthService
{
   
    Task<bool> IsUsernameExistsAsync(string username);
    
    Task<bool> IsEmailExistsAsync(string email);
    
    Task<IdentityResult> RegisterAsync(RegisterDto model);
    
    Task<string?> LoginAsync(LoginDto model);
}