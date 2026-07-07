using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.DTOs.Theater;

namespace P12_Movie_Reservation_System_Backend.Interfaces;

public interface ITheaterService
{
    Task<ApiResponse<List<TheaterListDto>>> GetAllTheatersAsync();

    Task<ApiResponse<TheaterDetailDto>> CreateTheaterAsync(CreateTheaterDto request);

    Task<ApiResponse<List<TheaterScreenDto>>> GetTheaterScreensAsync(int theaterId);

    Task<ApiResponse<List<TheaterListDto>>>GetTheatersByCityAsync(int cityId);
}