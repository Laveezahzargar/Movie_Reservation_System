using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P12_Movie_Reservation_System_Backend.Interfaces;

namespace P12_Movie_Reservation_System_Backend.Controllers;

[ApiController]
[Route("api/tickets")]
[Authorize]
public class TicketController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    // GET: /api/tickets/{bookingId}
    [HttpGet("{bookingId}")]
    public async Task<IActionResult> GetByBookingId(int bookingId)
    {
        var result = await _ticketService.GetTicketByBookingIdAsync(bookingId);
        return Ok(result);
    }

    // GET: /api/tickets/{bookingId}/download
    [HttpGet("{bookingId}/download")]
    public async Task<IActionResult> DownloadTicket(int bookingId)
    {
        var pdf = await _ticketService.DownloadTicketAsync(bookingId);

        if (pdf == null)
            return NotFound();

        return File(
            pdf,
            "application/pdf",
            $"Ticket_{bookingId}.pdf");
    }
}