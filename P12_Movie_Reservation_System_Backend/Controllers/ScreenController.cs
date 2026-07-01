using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P12_Movie_Reservation_System_Backend.DTOs.Screen;
using P12_Movie_Reservation_System_Backend.Interfaces;

namespace P12_Movie_Reservation_System_Backend.Controllers;

[ApiController]
[Route("api/screens")]
public class ScreenController : ControllerBase
{
    private readonly IScreenService _screenService;

    public ScreenController(IScreenService screenService)
    {
        _screenService = screenService;
    }

    // GET: api/screens
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _screenService.GetAllScreensAsync();
        return Ok(result);
    }

    // GET: api/screens/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _screenService.GetScreenByIdAsync(id);
        return Ok(result);
    }

    // POST: api/screens (ADMIN)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateScreenDto request)
    {
        var result = await _screenService.CreateScreenAsync(request);
        return Ok(result);
    }

    // PUT: api/screens/{id} (ADMIN)
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateScreenDto request)
    {
        var result = await _screenService.UpdateScreenAsync(id, request);
        return Ok(result);
    }

    // DELETE: api/screens/{id} (ADMIN)
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _screenService.DeleteScreenAsync(id);
        return Ok(result);
    }

    // GET: api/screens/{id}/seats
    [HttpGet("{id}/seats")]
    public async Task<IActionResult> GetSeats(int id)
    {
        var result = await _screenService.GetScreenSeatsAsync(id);
        return Ok(result);
    }

    // GET: api/screens/{id}/shows
    [HttpGet("{id}/shows")]
    public async Task<IActionResult> GetShows(int id)
    {
        var result = await _screenService.GetScreenShowsAsync(id);
        return Ok(result);
    }
}