using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.DTOs.Auth;

namespace P12_Movie_Reservation_System_Backend.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request);

    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request);
}
