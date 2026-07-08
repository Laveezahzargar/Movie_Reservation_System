using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P12_Movie_Reservation_System_Backend.DTOs.Seat;
using P12_Movie_Reservation_System_Backend.Interfaces;
using System.Security.Claims;

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

    [HttpGet("seats/show/{showId}")]
    public async Task<IActionResult> GetSeats(int showId)
    {
        var result = await _seatService.GetSeatsByShowAsync(showId);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("seats/reserve")]
    public async Task<IActionResult> Reserve(ReserveSeatDto request)
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _seatService
            .ReserveSeatAsync(request.ShowSeatId, userId);

        return Ok(result);
    }
    [Authorize]
    [HttpPost("seats/release")]
    public async Task<IActionResult> Release(ReleaseSeatDto request)
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _seatService
            .ReleaseSeatAsync(request.ShowSeatId, userId);

        return Ok(result);
    }
}