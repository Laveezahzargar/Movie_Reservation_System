using Microsoft.AspNetCore.Mvc;
using P12_Movie_Reservation_System_Backend.DTOs.Auth;
using P12_Movie_Reservation_System_Backend.Interfaces;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
    {
        var result =
            await _authService.RegisterAsync(dto);

        if (!result.Success)
            return BadRequest(result);

        return Created(string.Empty, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }
}