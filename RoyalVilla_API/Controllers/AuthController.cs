using RoyalVilla_API.Interfaces;

namespace RoyalVilla_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var result = await authService.RegisterAsync(model);

        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));

        return Ok(new { message = "Registered successfully." });
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var token = await authService.LoginAsync(model);

        if (token is null)
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(new { token });
    }

}