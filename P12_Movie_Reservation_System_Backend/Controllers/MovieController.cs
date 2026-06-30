using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P12_Movie_Reservation_System_Backend.DTOs.Movie;
using P12_Movie_Reservation_System_Backend.Interfaces;

namespace P12_Movie_Reservation_System_Backend.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    // GET: /api/movies
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _movieService.GetAllMoviesAsync();
        return Ok(result);
    }

    // GET: /api/movies/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _movieService.GetMovieByIdAsync(id);
        return Ok(result);
    }

    // POST: /api/movies (ADMIN)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateMovieDto request)
    {
        var result = await _movieService.CreateMovieAsync(request);
        return Ok(result);
    }

    // PUT: /api/movies/{id} (ADMIN)
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMovieDto request)
    {
        var result = await _movieService.UpdateMovieAsync(id, request);
        return Ok(result);
    }

    // DELETE: /api/movies/{id} (ADMIN)
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _movieService.DeleteMovieAsync(id);
        return Ok(result);
    }
}