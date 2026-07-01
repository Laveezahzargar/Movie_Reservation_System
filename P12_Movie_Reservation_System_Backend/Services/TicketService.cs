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

    public TicketService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<TicketDetailDto>> GetTicketByBookingIdAsync(int bookingId)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.BookingId == bookingId);

        if (ticket == null)
            return ApiResponse<TicketDetailDto>.FailureResponse("Ticket not found");

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
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.BookingId == bookingId);

        if (ticket == null)
            return ApiResponse<string>.FailureResponse("Ticket not found");

        // Simulated download link (you can later replace with real PDF generation)
        var downloadUrl = $"/tickets/download/{ticket.TicketNumber}.pdf";

        return ApiResponse<string>.SuccessResponse(downloadUrl, "Ticket download ready");
    }
}