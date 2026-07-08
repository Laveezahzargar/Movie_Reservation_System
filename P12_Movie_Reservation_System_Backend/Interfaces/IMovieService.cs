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

    Task<ApiResponse<List<MovieListDto>>>GetMoviesByCityAsync(int cityId);

    Task<ApiResponse<List<MovieListDto>>> SearchMoviesAsync(string keyword);

    Task<ApiResponse<List<MovieListDto>>> FilterMoviesAsync(string? genre,string? language);

    Task<ApiResponse<List<MovieListDto>>> GetNowShowingMoviesAsync();

    Task<ApiResponse<List<MovieListDto>>> GetUpcomingMoviesAsync();

    Task<ApiResponse<List<MovieListDto>>> GetRecommendedMoviesAsync();
}