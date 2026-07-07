using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.DTOs.Booking;
using P12_Movie_Reservation_System_Backend.Enums;
using P12_Movie_Reservation_System_Backend.Interfaces;
using P12_Movie_Reservation_System_Backend.Models.DomainModels;

namespace P12_Movie_Reservation_System_Backend.Services;

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BookingService> _logger;

    public BookingService(ApplicationDbContext context, ILogger<BookingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResponse<BookingDetailDto>> CreateBookingAsync(
        int userId,
        CreateBookingDto request)
    {
        _logger.LogInformation(
            "Creating booking for UserId {UserId}, ShowId {ShowId} with {SeatCount} seats.",
            userId,
            request.ShowId,
            request.ShowSeatIds.Count);

        var show = await _context.Shows
            .FirstOrDefaultAsync(s => s.ShowId == request.ShowId);

        if (show == null)
        {
            _logger.LogWarning(
                "Booking creation failed. ShowId {ShowId} not found.",
                request.ShowId);

            return ApiResponse<BookingDetailDto>
                .FailureResponse("Show not found");
        }

        var showSeats = await _context.ShowSeats
            .Include(ss => ss.Seat)
            .Where(ss =>
                request.ShowSeatIds.Contains(ss.ShowSeatId) &&
                ss.ShowId == request.ShowId)
            .ToListAsync();

        if (showSeats.Count != request.ShowSeatIds.Count)
        {
            _logger.LogWarning(
                "Booking creation failed. One or more ShowSeatIds are invalid for ShowId {ShowId}.",
                request.ShowId);

            return ApiResponse<BookingDetailDto>
                .FailureResponse("One or more seats are invalid");
        }

        if (showSeats.Any(s => s.Status != ShowSeatStatus.Available))
        {
            _logger.LogWarning(
                "Booking creation failed. One or more seats are already booked for ShowId {ShowId}.",
                request.ShowId);

            return ApiResponse<BookingDetailDto>
                .FailureResponse("One or more seats are already booked");
        }

        decimal seatPrice = 250m;

        var booking = new Booking
        {
            UserId = userId,
            ShowId = request.ShowId,
            MovieId = show.MovieId!.Value,
            TheaterId = show.TheaterId!.Value,
            ScreenId = show.ScreenId!.Value,
            BookingDate = DateTime.UtcNow,
            Status = "Confirmed",
            TotalAmount = seatPrice * request.ShowSeatIds.Count
        };

        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();

        foreach (var showSeat in showSeats)
        {
            await _context.BookingSeats.AddAsync(new BookingSeat
            {
                BookingId = booking.BookingId,
                ShowSeatId = showSeat.ShowSeatId
            });

            showSeat.Status = ShowSeatStatus.Booked;
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Booking created successfully. BookingId {BookingId}, UserId {UserId}, TotalAmount {TotalAmount}.",
            booking.BookingId,
            booking.UserId,
            booking.TotalAmount);

        var response = new BookingDetailDto
        {
            BookingId = booking.BookingId,
            UserId = booking.UserId,
            ShowId = booking.ShowId,
            BookingDate = booking.BookingDate,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            Seats = showSeats.Select(s => new BookingSeatDto
            {
                ShowSeatId = s.ShowSeatId,
                SeatId = s.SeatId,
                SeatNumber = s.Seat.Number,
                SeatType = s.Seat.Type.ToString()
            }).ToList()
        };

        return ApiResponse<BookingDetailDto>
            .SuccessResponse(response, "Booking Created Successfully");
    }

    public async Task<ApiResponse<List<BookingListDto>>> GetMyBookingsAsync(int userId)
    {
        _logger.LogInformation(
            "Fetching bookings for UserId {UserId}.",
            userId);

        var bookings = await _context.Bookings
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookingDate)
            .Select(b => new BookingListDto
            {
                BookingId = b.BookingId,
                ShowId = b.ShowId,
                BookingDate = b.BookingDate,
                TotalAmount = b.TotalAmount,
                Status = b.Status
            })
            .ToListAsync();

        _logger.LogInformation(
            "Retrieved {BookingCount} bookings for UserId {UserId}.",
            bookings.Count,
            userId);

        return ApiResponse<List<BookingListDto>>
            .SuccessResponse(bookings, "Bookings Retrieved Successfully");
    }

    public async Task<ApiResponse<BookingDetailDto>> GetBookingByIdAsync(int bookingId)
    {
        _logger.LogInformation(
            "Fetching booking with BookingId {BookingId}.",
            bookingId);

        var booking = await _context.Bookings
    .Include(b => b.Show)
        .ThenInclude(s => s.Movie)
    .Include(b => b.Show)
        .ThenInclude(s => s.Screen)
            .ThenInclude(sc => sc.Theater)
    .Include(b => b.BookingSeats)
        .ThenInclude(bs => bs.ShowSeat)
            .ThenInclude(ss => ss.Seat)
    .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        if (booking == null)
        {
            _logger.LogWarning(
                "Booking with BookingId {BookingId} was not found.",
                bookingId);

            return ApiResponse<BookingDetailDto>
                .FailureResponse("Booking not found");
        }

        var result = new BookingDetailDto
        {
            BookingId = booking.BookingId,
            UserId = booking.UserId,
            ShowId = booking.ShowId,

            MovieTitle = booking.Show.Movie.Title,
            TheaterName = booking.Show.Screen.Theater.TheaterName,
            ScreenName = booking.Show.Screen.ScreenName,
            ShowDateTime = booking.Show.ShowDateTime,

            BookingDate = booking.BookingDate,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,

            Seats = booking.BookingSeats.Select(bs => new BookingSeatDto
            {
                ShowSeatId = bs.ShowSeat.ShowSeatId,
                SeatId = bs.ShowSeat.SeatId,
                SeatNumber = bs.ShowSeat.Seat.Number,
                SeatType = bs.ShowSeat.Seat.Type.ToString()
            }).ToList()
        };

        _logger.LogInformation(
            "Booking with BookingId {BookingId} retrieved successfully.",
            booking.BookingId);

        return ApiResponse<BookingDetailDto>
            .SuccessResponse(result, "Booking Retrieved Successfully");
    }


    public async Task<ApiResponse<bool>> DeleteBookingAsync(int bookingId)
    {
        _logger.LogInformation(
            "Deleting booking with BookingId {BookingId}.",
            bookingId);

        var booking = await _context.Bookings
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.ShowSeat)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        if (booking == null)
        {
            _logger.LogWarning(
                "Booking deletion failed. BookingId {BookingId} not found.",
                bookingId);

            return ApiResponse<bool>
                .FailureResponse("Booking not found");
        }

        foreach (var bookingSeat in booking.BookingSeats)
        {
            bookingSeat.ShowSeat.Status = ShowSeatStatus.Available;
        }

        _context.BookingSeats.RemoveRange(booking.BookingSeats);
        _context.Bookings.Remove(booking);

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Booking with BookingId {BookingId} deleted successfully. Released {SeatCount} seats.",
            bookingId,
            booking.BookingSeats.Count);

        return ApiResponse<bool>
            .SuccessResponse(true, "Booking Deleted Successfully");
    }
}