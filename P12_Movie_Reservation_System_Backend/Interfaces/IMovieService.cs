using P12_Movie_Reservation_System_Backend.DTOs.Movie;
using P12_Movie_Reservation_System_Backend.Common;

namespace P12_Movie_Reservation_System_Backend.Interfaces;

public interface IMovieService
{
    Task<ApiResponse<List<MovieListDto>>> GetAllMoviesAsync();
    Task<ApiResponse<MovieDetailDto>> GetMovieByIdAsync(int id);

    Task<ApiResponse<MovieDetailDto>> CreateMovieAsync(CreateMovieDto request);

    Task<ApiResponse<MovieDetailDto>> UpdateMovieAsync(int id, UpdateMovieDto request);

    Task<ApiResponse<bool>> DeleteMovieAsync(int id);
}