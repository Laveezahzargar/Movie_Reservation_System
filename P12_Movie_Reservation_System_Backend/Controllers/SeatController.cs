using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P12_Movie_Reservation_System_Backend.DTOs.Seat;
using P12_Movie_Reservation_System_Backend.Interfaces;

namespace P12_Movie_Reservation_System_Backend.Controllers;

[ApiController]
[Route("api")]
public class SeatController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    // POST /api/seats
    [HttpPost("seats")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateSeatDto request)
    {
        var result = await _seatService.CreateSeatAsync(request);
        return Ok(result);
    }
}