using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P12_Movie_Reservation_System_Backend.DTOs.Show;
using P12_Movie_Reservation_System_Backend.Interfaces;

namespace P12_Movie_Reservation_System_Backend.Controllers;

[ApiController]
[Route("api/shows")]
public class ShowController : ControllerBase
{
    private readonly IShowService _showService;

    public ShowController(IShowService showService)
    {
        _showService = showService;
    }

    // GET /api/shows
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _showService.GetAllShowsAsync();
        return Ok(result);
    }

    // POST /api/shows
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateShowDto request)
    {
        var result = await _showService.CreateShowAsync(request);
        return Ok(result);
    }

    // GET /api/shows/{showId}/available-seats
    [HttpGet("{showId}/available-seats")]
    public async Task<IActionResult> GetAvailableSeats(int showId)
    {
        var result = await _showService.GetAvailableSeatsAsync(showId);
        return Ok(result);
    }

    [HttpGet("movie/{movieId}/city/{cityId}")]
    public async Task<IActionResult> GetShowsByMovieAndCity(
    int movieId,
    int cityId)
    {
        var result = await _showService
            .GetShowsByMovieAndCityAsync(movieId, cityId);

        return Ok(result);
    }
}