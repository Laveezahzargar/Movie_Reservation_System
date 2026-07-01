using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P12_Movie_Reservation_System_Backend.DTOs.Actor;
using P12_Movie_Reservation_System_Backend.Interfaces;

namespace P12_Movie_Reservation_System_Backend.Controllers;

[ApiController]
[Route("api/actors")]
public class ActorController : ControllerBase
{
    private readonly IActorService _actorService;

    public ActorController(IActorService actorService)
    {
        _actorService = actorService;
    }

    // GET: /api/actors
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _actorService.GetAllActorsAsync();
        return Ok(result);
    }

    // POST: /api/actors (ADMIN)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateActorDto request)
    {
        var result = await _actorService.CreateActorAsync(request);
        return Ok(result);
    }

    // GET: /api/actors/{id}/movies
    [HttpGet("{id}/movies")]
    public async Task<IActionResult> GetMovies(int id)
    {
        var result = await _actorService.GetActorMoviesAsync(id);
        return Ok(result);
    }
}