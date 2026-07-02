using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.DTOs.Ticket;
using P12_Movie_Reservation_System_Backend.Interfaces;
using P12_Movie_Reservation_System_Backend.Models.DomainModels;

namespace P12_Movie_Reservation_System_Backend.Services;

public class TicketService : ITicketService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TicketService> _logger;

    public TicketService(ApplicationDbContext context, ILogger<TicketService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResponse<TicketDetailDto>> GetTicketByBookingIdAsync(int bookingId)
    {
        _logger.LogInformation("Fetching ticket for BookingId {BookingId}.", bookingId);

        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.BookingId == bookingId);

        if (ticket == null)
        {
            _logger.LogWarning("No ticket found for BookingId {BookingId}.", bookingId);
            return ApiResponse<TicketDetailDto>.FailureResponse("Ticket not found");
        }

        _logger.LogInformation(
    "Ticket {TicketNumber} retrieved successfully for BookingId {BookingId}.",
    ticket.TicketNumber,
    bookingId);

        return ApiResponse<TicketDetailDto>.SuccessResponse(new TicketDetailDto
        {
            TicketId = ticket.TicketId,
            TicketNumber = ticket.TicketNumber,
            GeneratedAt = ticket.GeneratedAt,
            QRCodePath = ticket.QRCodePath,
            BookingId = ticket.BookingId
        }, "Ticket retrieved successfully");
    }

    public async Task<ApiResponse<string>> DownloadTicketAsync(int bookingId)
    {
        _logger.LogInformation("Preparing ticket download for BookingId {BookingId}.", bookingId);

        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.BookingId == bookingId);

        if (ticket == null)
        {
            _logger.LogWarning("Download failed. No ticket found for BookingId {BookingId}.", bookingId);
            return ApiResponse<string>.FailureResponse("Ticket not found");
        }

        // Simulated download link (you can later replace with real PDF generation)
        var downloadUrl = $"/tickets/download/{ticket.TicketNumber}.pdf";

        _logger.LogInformation(
    "Ticket download link generated successfully for TicketNumber {TicketNumber}.",
    ticket.TicketNumber);

        return ApiResponse<string>.SuccessResponse(downloadUrl, "Ticket download ready");
    }
}