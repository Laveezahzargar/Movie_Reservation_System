using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.DTOs.Ticket;
using P12_Movie_Reservation_System_Backend.Interfaces;

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
            .Include(t => t.Booking)
                .ThenInclude(b => b.Show)
                    .ThenInclude(s => s.Movie)

            .Include(t => t.Booking)
                .ThenInclude(b => b.Show)
                    .ThenInclude(s => s.Theater)

            .Include(t => t.Booking)
                .ThenInclude(b => b.Show)
                    .ThenInclude(s => s.Screen)

            .Include(t => t.Booking)
                .ThenInclude(b => b.BookingSeats)
                    .ThenInclude(bs => bs.ShowSeat)
                        .ThenInclude(ss => ss.Seat)

            .FirstOrDefaultAsync(t => t.BookingId == bookingId);

        if (ticket == null)
        {
            return ApiResponse<TicketDetailDto>.FailureResponse("Ticket not found");
        }

        var show = ticket.Booking.Show;

        return ApiResponse<TicketDetailDto>.SuccessResponse(new TicketDetailDto
        {
            TicketId = ticket.TicketId,
            TicketNumber = ticket.TicketNumber,
            GeneratedAt = ticket.GeneratedAt,
            QRCodePath = ticket.QRCodePath,
            BookingId = ticket.BookingId,

            MovieTitle = show.Movie.Title,
            TheaterName = show.Theater.TheaterName,
            ScreenName = show.Screen.ScreenName,

            // FIXED: your model uses ShowDateTime
            ShowDate = show.ShowDateTime.ToString("dd MMM yyyy"),
            ShowTime = show.ShowDateTime.ToString("hh:mm tt"),

            Seats = string.Join(", ",
                ticket.Booking.BookingSeats
                    .Select(x => x.ShowSeat.Seat.Number)),

            Amount = ticket.Booking.TotalAmount
        }, "Ticket retrieved successfully");
    }

    public async Task<ApiResponse<string>> DownloadTicketAsync(int bookingId)
    {
        _logger.LogInformation("Preparing ticket download for BookingId {BookingId}.", bookingId);

        var ticket = await _context.Tickets
            .Include(t => t.Booking)
                .ThenInclude(b => b.Show)
                    .ThenInclude(s => s.Movie)
            .Include(t => t.Booking)
                .ThenInclude(b => b.Show)
                    .ThenInclude(s => s.Theater)
            .Include(t => t.Booking)
                .ThenInclude(b => b.Show)
                    .ThenInclude(s => s.Screen)
            .Include(t => t.Booking)
                .ThenInclude(b => b.BookingSeats)
                    .ThenInclude(bs => bs.ShowSeat)
                        .ThenInclude(ss => ss.Seat)
            .FirstOrDefaultAsync(t => t.BookingId == bookingId);

        if (ticket == null)
        {
            return ApiResponse<string>.FailureResponse("Ticket not found");
        }

        var downloadUrl = $"/api/tickets/{ticket.BookingId}/download";

        return ApiResponse<string>.SuccessResponse(downloadUrl, "Ticket download ready");
    }
}