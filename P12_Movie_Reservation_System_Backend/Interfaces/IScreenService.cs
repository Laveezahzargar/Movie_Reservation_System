using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.DTOs.Screen;

namespace P12_Movie_Reservation_System_Backend.Interfaces;

public interface IScreenService
{
    Task<ApiResponse<List<ScreenListDto>>> GetAllScreensAsync();

    Task<ApiResponse<ScreenDetailDto>> GetScreenByIdAsync(int id);

    Task<ApiResponse<ScreenDetailDto>> CreateScreenAsync(CreateScreenDto request);

    Task<ApiResponse<ScreenDetailDto>> UpdateScreenAsync(int id, UpdateScreenDto request);

    Task<ApiResponse<bool>> DeleteScreenAsync(int id);

    Task<ApiResponse<List<ScreenSeatDto>>> GetScreenSeatsAsync(int screenId);

    Task<ApiResponse<List<ScreenShowDto>>> GetScreenShowsAsync(int screenId);
}