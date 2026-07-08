using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.DTOs.Seat;
using P12_Movie_Reservation_System_Backend.DTOs.Show;

namespace P12_Movie_Reservation_System_Backend.Interfaces;

public interface ISeatService
{
    Task<ApiResponse<SeatDetailDto>> CreateSeatAsync(CreateSeatDto request);

    Task<ApiResponse<bool>> ReserveSeatAsync(int showSeatId, int userId);

    Task<ApiResponse<bool>> ReleaseSeatAsync(int showSeatId, int userId);

    Task<ApiResponse<List<AvailableSeatDto>>> GetSeatsByShowAsync(int showId);
}