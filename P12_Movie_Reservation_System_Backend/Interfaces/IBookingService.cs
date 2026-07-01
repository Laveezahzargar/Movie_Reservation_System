using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.DTOs.Booking;

namespace P12_Movie_Reservation_System_Backend.Interfaces;

public interface IBookingService
{
    Task<ApiResponse<BookingDetailDto>> CreateBookingAsync(int userId, CreateBookingDto request);

    Task<ApiResponse<List<BookingListDto>>> GetMyBookingsAsync(int userId);

    Task<ApiResponse<BookingDetailDto>> GetBookingByIdAsync(int bookingId);

    Task<ApiResponse<bool>> DeleteBookingAsync(int bookingId);
}