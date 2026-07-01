using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P12_Movie_Reservation_System_Backend.DTOs.Booking;
using P12_Movie_Reservation_System_Backend.Interfaces;

namespace P12_Movie_Reservation_System_Backend.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    // POST: /api/bookings
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingDto request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                          ?? User.FindFirst(ClaimTypes.Name)
                          ?? User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized("Invalid token.");

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _bookingService.CreateBookingAsync(userId, request);

        return Ok(result);
    }

    // GET: /api/bookings/my-bookings
    [HttpGet("my-bookings")]
    public async Task<IActionResult> GetMyBookings()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _bookingService.GetMyBookingsAsync(userId);

        return Ok(result);
    }

    // GET: /api/bookings/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _bookingService.GetBookingByIdAsync(id);

        return Ok(result);
    }

    // DELETE: /api/bookings/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _bookingService.DeleteBookingAsync(id);

        return Ok(result);
    }
}