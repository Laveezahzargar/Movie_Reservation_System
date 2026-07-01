using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.DTOs.Seat;

namespace P12_Movie_Reservation_System_Backend.Interfaces;

public interface ISeatService
{
    Task<ApiResponse<SeatDetailDto>> CreateSeatAsync(CreateSeatDto request);
}