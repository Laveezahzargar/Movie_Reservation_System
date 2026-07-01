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

    public BookingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<BookingDetailDto>> CreateBookingAsync(
        int userId,
        CreateBookingDto request)
    {
        var show = await _context.Shows
            .FirstOrDefaultAsync(s => s.ShowId == request.ShowId);

        if (show == null)
            return ApiResponse<BookingDetailDto>
                .FailureResponse("Show not found");

        var showSeats = await _context.ShowSeats
            .Include(ss => ss.Seat)
            .Where(ss =>
                request.ShowSeatIds.Contains(ss.ShowSeatId) &&
                ss.ShowId == request.ShowId)
            .ToListAsync();

        if (showSeats.Count != request.ShowSeatIds.Count)
            return ApiResponse<BookingDetailDto>
                .FailureResponse("One or more seats are invalid");

        if (showSeats.Any(s => s.Status != ShowSeatStatus.Available))
            return ApiResponse<BookingDetailDto>
                .FailureResponse("One or more seats are already booked");

        decimal seatPrice = 250m;

        var booking = new Booking
        {
            UserId = userId,
            ShowId = request.ShowId,
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

        return ApiResponse<List<BookingListDto>>
            .SuccessResponse(bookings, "Bookings Retrieved Successfully");
    }

    public async Task<ApiResponse<BookingDetailDto>> GetBookingByIdAsync(int bookingId)
    {
        var booking = await _context.Bookings
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.ShowSeat)
                    .ThenInclude(ss => ss.Seat)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        if (booking == null)
            return ApiResponse<BookingDetailDto>
                .FailureResponse("Booking not found");

        var result = new BookingDetailDto
        {
            BookingId = booking.BookingId,
            UserId = booking.UserId,
            ShowId = booking.ShowId,
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

        return ApiResponse<BookingDetailDto>
            .SuccessResponse(result, "Booking Retrieved Successfully");
    }

    public async Task<ApiResponse<bool>> DeleteBookingAsync(int bookingId)
    {
        var booking = await _context.Bookings
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.ShowSeat)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        if (booking == null)
            return ApiResponse<bool>
                .FailureResponse("Booking not found");

        foreach (var bookingSeat in booking.BookingSeats)
        {
            bookingSeat.ShowSeat.Status = ShowSeatStatus.Available;
        }

        _context.BookingSeats.RemoveRange(booking.BookingSeats);
        _context.Bookings.Remove(booking);

        await _context.SaveChangesAsync();

        return ApiResponse<bool>
            .SuccessResponse(true, "Booking Deleted Successfully");
    }
}