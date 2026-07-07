using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.DTOs.Show;

namespace P12_Movie_Reservation_System_Backend.Interfaces;

public interface IShowService
{
    Task<ApiResponse<List<ShowListDto>>> GetAllShowsAsync();

    Task<ApiResponse<ShowDetailDto>> CreateShowAsync(CreateShowDto request);

    Task<ApiResponse<List<AvailableSeatDto>>> GetAvailableSeatsAsync(int showId);

    Task<ApiResponse<List<ShowListDto>>>GetShowsByMovieAndCityAsync(int movieId,int cityId);
}