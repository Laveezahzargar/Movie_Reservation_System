using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.DTOs.Show;
using P12_Movie_Reservation_System_Backend.Enums;
using P12_Movie_Reservation_System_Backend.Interfaces;
using P12_Movie_Reservation_System_Backend.Models.DomainModels;

namespace P12_Movie_Reservation_System_Backend.Services;

public class ShowService : IShowService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ShowService> _logger;

    public ShowService(ApplicationDbContext context, ILogger<ShowService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResponse<List<ShowListDto>>> GetAllShowsAsync()
    {
        _logger.LogInformation("Fetching all shows.");

        var shows = await _context.Shows
            .Include(s => s.Movie)
            .Include(s => s.Screen)
                .ThenInclude(sc => sc.Theater)
            .ToListAsync();

        var result = shows.Select(s => new ShowListDto
        {
            ShowId = s.ShowId,

            MovieId = (int)s.MovieId,
            MovieTitle = s.Movie.Title,

            TheaterId = s.Screen.Theater.TheaterId,
            TheaterName = s.Screen.Theater.TheaterName,

            ScreenId = (int)s.ScreenId,
            ScreenName = s.Screen.ScreenName,

            ShowDateTime = s.ShowDateTime

        }).ToList();

        _logger.LogInformation(
            "Retrieved {ShowCount} shows successfully.",
            result.Count);

        return ApiResponse<List<ShowListDto>>
            .SuccessResponse(result, "Shows Retrieved Successfully");
    }

    public async Task<ApiResponse<ShowDetailDto>> CreateShowAsync(CreateShowDto request)
    {
        _logger.LogInformation(
            "Creating show for MovieId {MovieId} on ScreenId {ScreenId} scheduled at {ShowDateTime}.",
            request.MovieId,
            request.ScreenId,
            request.ShowDateTime);

        var movieExists = await _context.Movies
            .AnyAsync(m => m.MovieId == request.MovieId);

        if (!movieExists)
        {
            _logger.LogWarning(
                "Show creation failed. MovieId {MovieId} not found.",
                request.MovieId);

            return ApiResponse<ShowDetailDto>
                .FailureResponse("Movie not found");
        }

        var screenExists = await _context.Screens
            .AnyAsync(s => s.ScreenId == request.ScreenId);

        if (!screenExists)
        {
            _logger.LogWarning(
                "Show creation failed. ScreenId {ScreenId} not found.",
                request.ScreenId);

            return ApiResponse<ShowDetailDto>
                .FailureResponse("Screen not found");
        }

        var show = new Show
        {
            MovieId = request.MovieId,
            ScreenId = request.ScreenId,
            ShowDateTime = request.ShowDateTime
        };

        await _context.Shows.AddAsync(show);
        await _context.SaveChangesAsync();

        await _context.Entry(show)
    .Reference(s => s.Movie)
    .LoadAsync();

        await _context.Entry(show)
            .Reference(s => s.Screen)
            .LoadAsync();

        await _context.Entry(show.Screen)
            .Reference(s => s.Theater)
            .LoadAsync();

        _logger.LogInformation(
            "Show created successfully. ShowId {ShowId}. Generating ShowSeats.",
            show.ShowId);

        var seats = await _context.Seats
            .Where(s => s.ScreenId == show.ScreenId)
            .ToListAsync();

        foreach (var seat in seats)
        {
            await _context.ShowSeats.AddAsync(new ShowSeat
            {
                ShowId = show.ShowId,
                SeatId = seat.SeatId,
                Status = ShowSeatStatus.Available
            });
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "{SeatCount} ShowSeats created successfully for ShowId {ShowId}.",
            seats.Count,
            show.ShowId);

        return ApiResponse<ShowDetailDto>.SuccessResponse(
            new ShowDetailDto
            {
                ShowId = show.ShowId,
                MovieId = (int)show.MovieId,
                ScreenId = (int)show.ScreenId,
                ShowDateTime = show.ShowDateTime
            },
            "Show Created Successfully");
    }
    public async Task<ApiResponse<List<ShowListDto>>>GetShowsByMovieAndCityAsync( int movieId, int cityId)
    {

        var shows = await _context.Shows

            .Where(s =>
                s.MovieId == movieId &&
                s.Theater.CityId == cityId)

            .Select(s => new ShowListDto
            {
                ShowId = s.ShowId,

                MovieId = s.MovieId.Value,

                TheaterId = s.TheaterId.Value,

                TheaterName = s.Theater.TheaterName,

                ShowDateTime = s.ShowDateTime,

                ScreenName = s.Screen.ScreenName
            })

            .ToListAsync();


        return ApiResponse<List<ShowListDto>>
            .SuccessResponse(
                shows,
                "Shows retrieved successfully");
    }
    public async Task<ApiResponse<List<ShowListDto>>> GetShowsByMovieAsync(int movieId)
    {
        _logger.LogInformation(
            "Fetching shows for MovieId {MovieId}.",
            movieId);

        var movieExists = await _context.Movies
            .AnyAsync(m => m.MovieId == movieId);

        if (!movieExists)
        {
            _logger.LogWarning(
                "MovieId {MovieId} not found.",
                movieId);

            return ApiResponse<List<ShowListDto>>
                .FailureResponse("Movie not found.");
        }

        var shows = await _context.Shows
            .Where(s => s.MovieId == movieId)
            .Include(s => s.Movie)
            .Include(s => s.Screen)
                .ThenInclude(sc => sc.Theater)
            .OrderBy(s => s.ShowDateTime)
            .Select(s => new ShowListDto
            {
                ShowId = s.ShowId,
                MovieId = s.MovieId!.Value,
                MovieTitle = s.Movie!.Title,
                TheaterId = s.Screen.Theater.TheaterId,
                TheaterName = s.Screen.Theater.TheaterName,
                ScreenId = s.ScreenId!.Value,
                ScreenName = s.Screen.ScreenName,
                ShowDateTime = s.ShowDateTime
            })
            .ToListAsync();

        _logger.LogInformation(
            "Retrieved {ShowCount} shows for MovieId {MovieId}.",
            shows.Count,
            movieId);

        return ApiResponse<List<ShowListDto>>
            .SuccessResponse(shows, "Shows retrieved successfully.");
    }
    public async Task<ApiResponse<List<DateTime>>> GetShowDatesByMovieAsync(int movieId)
    {
        _logger.LogInformation(
            "Fetching available show dates for MovieId {MovieId}.",
            movieId);

        var movieExists = await _context.Movies
            .AnyAsync(m => m.MovieId == movieId);

        if (!movieExists)
        {
            _logger.LogWarning(
                "MovieId {MovieId} not found.",
                movieId);

            return ApiResponse<List<DateTime>>
                .FailureResponse("Movie not found.");
        }

        var dates = await _context.Shows
            .Where(s => s.MovieId == movieId)
            .OrderBy(s => s.ShowDateTime)
            .Select(s => s.ShowDateTime.Date)
            .Distinct()
            .ToListAsync();

        _logger.LogInformation(
            "Retrieved {DateCount} unique show dates for MovieId {MovieId}.",
            dates.Count,
            movieId);

        return ApiResponse<List<DateTime>>
            .SuccessResponse(dates, "Show dates retrieved successfully.");
    }
}