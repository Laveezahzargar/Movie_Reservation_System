using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.DTOs.Movie;
using P12_Movie_Reservation_System_Backend.Interfaces;
using P12_Movie_Reservation_System_Backend.Models.DomainModels;

namespace P12_Movie_Reservation_System_Backend.Services;

public class MovieService : IMovieService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MovieService> _logger;

    public MovieService(ApplicationDbContext context, ILogger<MovieService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResponse<List<MovieListDto>>> GetAllMoviesAsync()
    {
        _logger.LogInformation("Fetching all movies.");

        var movies = await _context.Movies.ToListAsync();

        var result = movies.Select(m => new MovieListDto
        {
            MovieId = m.MovieId,
            Title = m.Title,
            Genre = m.Genre
        }).ToList();

        _logger.LogInformation(
            "Retrieved {MovieCount} movies successfully.",
            result.Count);

        return ApiResponse<List<MovieListDto>>
            .SuccessResponse(result, "Movies Retrieved Successfully");
    }

    public async Task<ApiResponse<MovieDetailDto>> GetMovieByIdAsync(int id)
    {
        _logger.LogInformation(
            "Fetching movie with MovieId {MovieId}.",
            id);

        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
        {
            _logger.LogWarning(
                "Movie with MovieId {MovieId} was not found.",
                id);

            return ApiResponse<MovieDetailDto>
                .FailureResponse("Movie not found");
        }

        _logger.LogInformation(
            "Movie with MovieId {MovieId} retrieved successfully.",
            movie.MovieId);

        return ApiResponse<MovieDetailDto>.SuccessResponse(
            new MovieDetailDto
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                Genre = movie.Genre,
                Duration = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate
            },
            "Movie Retrieved Successfully");
    }

    public async Task<ApiResponse<MovieDetailDto>> CreateMovieAsync(CreateMovieDto request)
    {
        _logger.LogInformation(
            "Creating movie '{MovieTitle}'.",
            request.Title);

        var movie = new Movie
        {
            Title = request.Title,
            Genre = request.Genre,
            DurationMinutes = request.Duration,
            Description = request.Description,
            ReleaseDate = request.ReleaseDate
        };

        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Movie created successfully. MovieId {MovieId}, Title '{MovieTitle}'.",
            movie.MovieId,
            movie.Title);

        return ApiResponse<MovieDetailDto>.SuccessResponse(
            new MovieDetailDto
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                Genre = movie.Genre,
                Duration = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate
            },
            "Movie Created Successfully");
    }

    public async Task<ApiResponse<MovieDetailDto>> UpdateMovieAsync(int id, UpdateMovieDto request)
    {
        _logger.LogInformation(
            "Updating movie with MovieId {MovieId}.",
            id);

        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
        {
            _logger.LogWarning(
                "Movie update failed. MovieId {MovieId} not found.",
                id);

            return ApiResponse<MovieDetailDto>
                .FailureResponse("Movie not found");
        }

        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.Genre = request.Genre;
        movie.DurationMinutes = request.Duration;
        movie.ReleaseDate = request.ReleaseDate;

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Movie with MovieId {MovieId} updated successfully.",
            movie.MovieId);

        return ApiResponse<MovieDetailDto>.SuccessResponse(
            new MovieDetailDto
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                Genre = movie.Genre,
                Duration = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate
            },
            "Movie Updated Successfully");
    }

    public async Task<ApiResponse<bool>> DeleteMovieAsync(int id)
    {
        _logger.LogInformation(
            "Deleting movie with MovieId {MovieId}.",
            id);

        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
        {
            _logger.LogWarning(
                "Movie deletion failed. MovieId {MovieId} not found.",
                id);

            return ApiResponse<bool>
                .FailureResponse("Movie not found");
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Movie with MovieId {MovieId} deleted successfully.",
            id);

        return ApiResponse<bool>
            .SuccessResponse(true, "Movie Deleted Successfully");
    }
}