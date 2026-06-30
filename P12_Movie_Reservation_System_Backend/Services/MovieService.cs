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

    public MovieService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<MovieListDto>>> GetAllMoviesAsync()
    {
        var movies = await _context.Movies.ToListAsync();

        var result = movies.Select(m => new MovieListDto
        {
            MovieId = m.MovieId,
            Title = m.Title,
            Genre = m.Genre
        }).ToList();

        return ApiResponse<List<MovieListDto>>
            .SuccessResponse(result, "Movies Retrieved Successfully");
    }

    public async Task<ApiResponse<MovieDetailDto>> GetMovieByIdAsync(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
            return ApiResponse<MovieDetailDto>.FailureResponse("Movie not found");

        return ApiResponse<MovieDetailDto>.SuccessResponse(new MovieDetailDto
        {
            MovieId = movie.MovieId,
            Title = movie.Title,
            Description = movie.Description,
            Genre = movie.Genre,
            Duration = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate
        }, "Movie Retrieved Successfully");
    }

    public async Task<ApiResponse<MovieDetailDto>> CreateMovieAsync(CreateMovieDto request)
    {
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

        return ApiResponse<MovieDetailDto>.SuccessResponse(new MovieDetailDto
        {
            MovieId = movie.MovieId,
            Title = movie.Title,
            Description = movie.Description,
            Genre = movie.Genre,
            Duration = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate
        }, "Movie Created Successfully");
    }

    public async Task<ApiResponse<MovieDetailDto>> UpdateMovieAsync(int id, UpdateMovieDto request)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
            return ApiResponse<MovieDetailDto>.FailureResponse("Movie not found");

        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.Genre = request.Genre;
        movie.DurationMinutes = request.Duration;
        movie.ReleaseDate = request.ReleaseDate;

        await _context.SaveChangesAsync();

        return ApiResponse<MovieDetailDto>.SuccessResponse(new MovieDetailDto
        {
            MovieId = movie.MovieId,
            Title = movie.Title,
            Description = movie.Description,
            Genre = movie.Genre,
            Duration = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate
        }, "Movie Updated Successfully");
    }

    public async Task<ApiResponse<bool>> DeleteMovieAsync(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
            return ApiResponse<bool>.FailureResponse("Movie not found");

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Movie Deleted Successfully");
    }
}