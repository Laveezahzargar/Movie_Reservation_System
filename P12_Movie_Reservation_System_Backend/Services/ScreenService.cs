using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.DTOs.Screen;
using P12_Movie_Reservation_System_Backend.Interfaces;
using P12_Movie_Reservation_System_Backend.Models.DomainModels;

namespace P12_Movie_Reservation_System_Backend.Services;

public class ScreenService : IScreenService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ScreenService> _logger;

    public ScreenService(ApplicationDbContext context, ILogger<ScreenService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResponse<List<ScreenListDto>>> GetAllScreensAsync()
    {
        _logger.LogInformation("Fetching all screens.");

        var screens = await _context.Screens.ToListAsync();

        var result = screens.Select(s => new ScreenListDto
        {
            ScreenId = s.ScreenId,
            ScreenName = s.ScreenName,
            TheaterId = s.TheaterId
        }).ToList();

        _logger.LogInformation(
            "Retrieved {ScreenCount} screens successfully.",
            result.Count);

        return ApiResponse<List<ScreenListDto>>
            .SuccessResponse(result, "Screens Retrieved Successfully");
    }

    public async Task<ApiResponse<ScreenDetailDto>> GetScreenByIdAsync(int id)
    {
        _logger.LogInformation(
            "Fetching screen with ScreenId {ScreenId}.",
            id);

        var screen = await _context.Screens.FindAsync(id);

        if (screen == null)
        {
            _logger.LogWarning(
                "Screen with ScreenId {ScreenId} was not found.",
                id);

            return ApiResponse<ScreenDetailDto>
                .FailureResponse("Screen not found");
        }

        _logger.LogInformation(
            "Screen with ScreenId {ScreenId} retrieved successfully.",
            screen.ScreenId);

        return ApiResponse<ScreenDetailDto>.SuccessResponse(
            new ScreenDetailDto
            {
                ScreenId = screen.ScreenId,
                ScreenName = screen.ScreenName,
                TheaterId = screen.TheaterId
            },
            "Screen Retrieved Successfully");
    }

    public async Task<ApiResponse<ScreenDetailDto>> CreateScreenAsync(CreateScreenDto request)
    {
        _logger.LogInformation(
            "Creating screen '{ScreenName}' for TheaterId {TheaterId}.",
            request.ScreenName,
            request.TheaterId);

        var theaterExists = await _context.Theaters
            .AnyAsync(t => t.TheaterId == request.TheaterId);

        if (!theaterExists)
        {
            _logger.LogWarning(
                "Screen creation failed. TheaterId {TheaterId} not found.",
                request.TheaterId);

            return ApiResponse<ScreenDetailDto>
                .FailureResponse("Theater not found");
        }

        var screen = new Screen
        {
            ScreenName = request.ScreenName,
            TheaterId = request.TheaterId
        };

        await _context.Screens.AddAsync(screen);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Screen created successfully. ScreenId {ScreenId}, ScreenName {ScreenName}.",
            screen.ScreenId,
            screen.ScreenName);

        return ApiResponse<ScreenDetailDto>.SuccessResponse(
            new ScreenDetailDto
            {
                ScreenId = screen.ScreenId,
                ScreenName = screen.ScreenName,
                TheaterId = screen.TheaterId
            },
            "Screen Created Successfully");
    }

    public async Task<ApiResponse<ScreenDetailDto>> UpdateScreenAsync(int id, UpdateScreenDto request)
    {
        _logger.LogInformation(
            "Updating screen with ScreenId {ScreenId}.",
            id);

        var screen = await _context.Screens.FindAsync(id);

        if (screen == null)
        {
            _logger.LogWarning(
                "Screen update failed. ScreenId {ScreenId} not found.",
                id);

            return ApiResponse<ScreenDetailDto>
                .FailureResponse("Screen not found");
        }

        var theaterExists = await _context.Theaters
            .AnyAsync(t => t.TheaterId == request.TheaterId);

        if (!theaterExists)
        {
            _logger.LogWarning(
                "Screen update failed. TheaterId {TheaterId} not found.",
                request.TheaterId);

            return ApiResponse<ScreenDetailDto>
                .FailureResponse("Theater not found");
        }

        screen.ScreenName = request.ScreenName;
        screen.TheaterId = request.TheaterId;

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Screen with ScreenId {ScreenId} updated successfully.",
            screen.ScreenId);

        return ApiResponse<ScreenDetailDto>.SuccessResponse(
            new ScreenDetailDto
            {
                ScreenId = screen.ScreenId,
                ScreenName = screen.ScreenName,
                TheaterId = screen.TheaterId
            },
            "Screen Updated Successfully");
    }

    public async Task<ApiResponse<bool>> DeleteScreenAsync(int id)
    {
        _logger.LogInformation(
            "Deleting screen with ScreenId {ScreenId}.",
            id);

        var screen = await _context.Screens.FindAsync(id);

        if (screen == null)
        {
            _logger.LogWarning(
                "Screen deletion failed. ScreenId {ScreenId} not found.",
                id);

            return ApiResponse<bool>
                .FailureResponse("Screen not found");
        }

        _context.Screens.Remove(screen);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Screen with ScreenId {ScreenId} deleted successfully.",
            id);

        return ApiResponse<bool>
            .SuccessResponse(true, "Screen Deleted Successfully");
    }

    public async Task<ApiResponse<List<ScreenSeatDto>>> GetScreenSeatsAsync(int screenId)
    {
        _logger.LogInformation(
            "Fetching seats for ScreenId {ScreenId}.",
            screenId);

        var screenExists = await _context.Screens
            .AnyAsync(s => s.ScreenId == screenId);

        if (!screenExists)
        {
            _logger.LogWarning(
                "Screen with ScreenId {ScreenId} was not found.",
                screenId);

            return ApiResponse<List<ScreenSeatDto>>
                .FailureResponse("Screen not found");
        }

        var seats = await _context.Seats
            .Where(s => s.ScreenId == screenId)
            .Select(s => new ScreenSeatDto
            {
                SeatId = s.SeatId,
                SeatNumber = s.Number,
                SeatType = s.Type.ToString()
            })
            .ToListAsync();

        _logger.LogInformation(
            "Retrieved {SeatCount} seats for ScreenId {ScreenId}.",
            seats.Count,
            screenId);

        return ApiResponse<List<ScreenSeatDto>>
            .SuccessResponse(seats, "Seats Retrieved Successfully");
    }

    public async Task<ApiResponse<List<ScreenShowDto>>> GetScreenShowsAsync(int screenId)
    {
        _logger.LogInformation(
            "Fetching shows for ScreenId {ScreenId}.",
            screenId);

        var screenExists = await _context.Screens
            .AnyAsync(s => s.ScreenId == screenId);

        if (!screenExists)
        {
            _logger.LogWarning(
                "Screen with ScreenId {ScreenId} was not found.",
                screenId);

            return ApiResponse<List<ScreenShowDto>>
                .FailureResponse("Screen not found");
        }

        var shows = await _context.Shows
            .Where(s => s.ScreenId == screenId)
            .Select(s => new ScreenShowDto
            {
                ShowId = s.ShowId,
                MovieId = s.MovieId,
                ShowTime = s.ShowDateTime
            })
            .ToListAsync();

        _logger.LogInformation(
            "Retrieved {ShowCount} shows for ScreenId {ScreenId}.",
            shows.Count,
            screenId);

        return ApiResponse<List<ScreenShowDto>>
            .SuccessResponse(shows, "Shows Retrieved Successfully");
    }
}