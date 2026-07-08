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
    private static MovieListDto MapMovie(Movie movie)
    {
        return new MovieListDto
        {
            MovieId = movie.MovieId,
            Title = movie.Title,
            Genre = movie.Genre,
            Language = movie.Language,
            Duration = movie.DurationMinutes,
            Rating = movie.Rating,
            PosterUrl = movie.PosterUrl,
            IsFeatured = movie.IsFeatured
        };
    }
    public async Task<ApiResponse<List<MovieListDto>>> GetAllMoviesAsync()
    {
        _logger.LogInformation("Fetching all movies.");

        var movies = await _context.Movies.Where(m => m.IsActive).ToListAsync();

        var result = movies.Select(m => new MovieListDto
        {
            MovieId = m.MovieId,
            Title = m.Title,
            Genre = m.Genre,
            Language = m.Language,
            Duration = m.DurationMinutes,
            Rating = m.Rating,
            PosterUrl = m.PosterUrl,
            IsFeatured = m.IsFeatured
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

        var movie = await _context.Movies.FirstOrDefaultAsync(m =>m.MovieId == id &&m.IsActive);

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
                Language = movie.Language,
                Duration = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate,
                Rating = movie.Rating,
                Certificate = movie.Certificate,
                Director = movie.Director,
                PosterUrl = movie.PosterUrl,
                TrailerUrl = movie.TrailerUrl,
                IsFeatured = movie.IsFeatured
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
            Description = request.Description,
            Genre = request.Genre,
            Language = request.Language,
            DurationMinutes = request.Duration,
            ReleaseDate = request.ReleaseDate,
            Rating = request.Rating,
            Certificate = request.Certificate,
            Director = request.Director,
            PosterUrl = request.PosterUrl,
            TrailerUrl = request.TrailerUrl,
            IsFeatured = request.IsFeatured,
            IsActive = true
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
                 Language = movie.Language,
                 Duration = movie.DurationMinutes,
                 ReleaseDate = movie.ReleaseDate,
                 Rating = movie.Rating,
                 Certificate = movie.Certificate,
                 Director = movie.Director,
                 PosterUrl = movie.PosterUrl,
                 TrailerUrl = movie.TrailerUrl,
                 IsFeatured = movie.IsFeatured
             },
            "Movie Created Successfully");
    }

    public async Task<ApiResponse<MovieDetailDto>> UpdateMovieAsync(int id, UpdateMovieDto request)
    {
        _logger.LogInformation(
            "Updating movie with MovieId {MovieId}.",
            id);

        var movie = await _context.Movies
    .FirstOrDefaultAsync(m =>
        m.MovieId == id &&
        m.IsActive);

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
        movie.Language = request.Language;
        movie.DurationMinutes = request.Duration;
        movie.ReleaseDate = request.ReleaseDate;
        movie.Rating = request.Rating;
        movie.Certificate = request.Certificate;
        movie.Director = request.Director;
        movie.PosterUrl = request.PosterUrl;
        movie.TrailerUrl = request.TrailerUrl;
        movie.IsFeatured = request.IsFeatured;
        movie.IsActive = request.IsActive;

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
                Language = movie.Language,
                Duration = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate,
                Rating = movie.Rating,
                Certificate = movie.Certificate,
                Director = movie.Director,
                PosterUrl = movie.PosterUrl,
                TrailerUrl = movie.TrailerUrl,
                IsFeatured = movie.IsFeatured
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

        movie.IsActive = false;
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Movie with MovieId {MovieId} deleted successfully.",
            id);

        return ApiResponse<bool>
            .SuccessResponse(true, "Movie Deleted Successfully");
    }
    public async Task<ApiResponse<List<MovieListDto>>>GetMoviesByCityAsync(int cityId)
    {
        var movies = await _context.Movies
            .Where(m =>
    m.IsActive &&
    m.Shows.Any(s => s.Theater.CityId == cityId))
            .Select(m => new MovieListDto
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Genre = m.Genre,
                Language = m.Language,
                Duration = m.DurationMinutes,
                Rating = m.Rating,
                PosterUrl = m.PosterUrl,
                IsFeatured = m.IsFeatured
            })
            .Distinct()
            .ToListAsync();


        return ApiResponse<List<MovieListDto>>
            .SuccessResponse(
                movies,
                "Movies fetched successfully");
    }
    public async Task<ApiResponse<List<MovieListDto>>> SearchMoviesAsync(string keyword)
    {
        _logger.LogInformation(
            "Searching movies using keyword '{Keyword}'.",
            keyword);

        keyword = keyword.Trim();

        var movies = await _context.Movies
            .Where(m =>
                m.IsActive &&
                (m.Title.Contains(keyword) ||
                 m.Description.Contains(keyword) ||
                 m.Genre.Contains(keyword)))
            .ToListAsync();

        var result = movies
            .Select(MapMovie)
            .ToList();

        return ApiResponse<List<MovieListDto>>
            .SuccessResponse(result,
                "Movies retrieved successfully.");
    }
    public async Task<ApiResponse<List<MovieListDto>>> FilterMoviesAsync(
    string? genre,
    string? language)
    {
        _logger.LogInformation(
            "Filtering movies. Genre: {Genre}, Language: {Language}.",
            genre,
            language);

        var query = _context.Movies
            .Where(m => m.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(genre))
        {
            query = query.Where(m => m.Genre == genre);
        }

        if (!string.IsNullOrWhiteSpace(language))
        {
            query = query.Where(m => m.Language == language);
        }

        var movies = await query.ToListAsync();

        return ApiResponse<List<MovieListDto>>
            .SuccessResponse(
                movies.Select(MapMovie).ToList(),
                "Movies retrieved successfully.");
    }

    public async Task<ApiResponse<List<MovieListDto>>> GetNowShowingMoviesAsync()
    {
        _logger.LogInformation(
            "Fetching now showing movies.");

        var today = DateTime.Today;

        var movies = await _context.Movies
            .Where(m =>
                m.IsActive &&
                m.Shows.Any(s =>
                    s.ShowDateTime >= today))
            .Distinct()
            .ToListAsync();

        return ApiResponse<List<MovieListDto>>
            .SuccessResponse(
                movies.Select(MapMovie).ToList(),
                "Now showing movies retrieved successfully.");
    }
    public async Task<ApiResponse<List<MovieListDto>>> GetUpcomingMoviesAsync()
    {
        _logger.LogInformation(
            "Fetching upcoming movies.");

        var today = DateTime.Today;

        var movies = await _context.Movies
            .Where(m =>
                m.IsActive &&
                m.ReleaseDate > today)
            .OrderBy(m => m.ReleaseDate)
            .ToListAsync();

        return ApiResponse<List<MovieListDto>>
            .SuccessResponse(
                movies.Select(MapMovie).ToList(),
                "Upcoming movies retrieved successfully.");
    }
    public async Task<ApiResponse<List<MovieListDto>>> GetRecommendedMoviesAsync()
    {
        _logger.LogInformation(
            "Fetching recommended movies.");

        var movies = await _context.Movies
            .Where(m => m.IsActive)
            .OrderByDescending(m => m.IsFeatured)
            .ThenByDescending(m => m.Rating)
            .ThenByDescending(m => m.ReleaseDate)
            .Take(10)
            .ToListAsync();

        return ApiResponse<List<MovieListDto>>
            .SuccessResponse(
                movies.Select(MapMovie).ToList(),
                "Recommended movies retrieved successfully.");
    }
}