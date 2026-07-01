using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P12_Movie_Reservation_System_Backend.DTOs.Theater;
using P12_Movie_Reservation_System_Backend.Interfaces;

namespace P12_Movie_Reservation_System_Backend.Controllers;

[ApiController]
[Route("api/theaters")]
public class TheaterController : ControllerBase
{
    private readonly ITheaterService _theaterService;

    public TheaterController(ITheaterService theaterService)
    {
        _theaterService = theaterService;
    }

    // GET: api/theaters
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _theaterService.GetAllTheatersAsync();
        return Ok(result);
    }

    // POST: api/theaters (ADMIN)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateTheaterDto request)
    {
        var result = await _theaterService.CreateTheaterAsync(request);
        return Ok(result);
    }

    // GET: api/theaters/{id}/screens
    [HttpGet("{id}/screens")]
    public async Task<IActionResult> GetScreens(int id)
    {
        var result = await _theaterService.GetTheaterScreensAsync(id);
        return Ok(result);
    }
}