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

    public ScreenService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<ScreenListDto>>> GetAllScreensAsync()
    {
        var screens = await _context.Screens.ToListAsync();

        var result = screens.Select(s => new ScreenListDto
        {
            ScreenId = s.ScreenId,
            ScreenName = s.ScreenName,
            TheaterId = s.TheaterId
        }).ToList();

        return ApiResponse<List<ScreenListDto>>
            .SuccessResponse(result, "Screens Retrieved Successfully");
    }

    public async Task<ApiResponse<ScreenDetailDto>> GetScreenByIdAsync(int id)
    {
        var screen = await _context.Screens.FindAsync(id);

        if (screen == null)
            return ApiResponse<ScreenDetailDto>
                .FailureResponse("Screen not found");

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
        var theaterExists = await _context.Theaters
            .AnyAsync(t => t.TheaterId == request.TheaterId);

        if (!theaterExists)
            return ApiResponse<ScreenDetailDto>
                .FailureResponse("Theater not found");

        var screen = new Screen
        {
            ScreenName = request.ScreenName,
            TheaterId = request.TheaterId
        };

        await _context.Screens.AddAsync(screen);
        await _context.SaveChangesAsync();

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
        var screen = await _context.Screens.FindAsync(id);

        if (screen == null)
            return ApiResponse<ScreenDetailDto>
                .FailureResponse("Screen not found");

        var theaterExists = await _context.Theaters
            .AnyAsync(t => t.TheaterId == request.TheaterId);

        if (!theaterExists)
            return ApiResponse<ScreenDetailDto>
                .FailureResponse("Theater not found");

        screen.ScreenName = request.ScreenName;
        screen.TheaterId = request.TheaterId;

        await _context.SaveChangesAsync();

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
        var screen = await _context.Screens.FindAsync(id);

        if (screen == null)
            return ApiResponse<bool>
                .FailureResponse("Screen not found");

        _context.Screens.Remove(screen);
        await _context.SaveChangesAsync();

        return ApiResponse<bool>
            .SuccessResponse(true, "Screen Deleted Successfully");
    }

    public async Task<ApiResponse<List<ScreenSeatDto>>> GetScreenSeatsAsync(int screenId)
    {
        var screenExists = await _context.Screens
            .AnyAsync(s => s.ScreenId == screenId);

        if (!screenExists)
            return ApiResponse<List<ScreenSeatDto>>
                .FailureResponse("Screen not found");

        var seats = await _context.Seats
            .Where(s => s.ScreenId == screenId)
            .Select(s => new ScreenSeatDto
            {
                SeatId = s.SeatId,
                SeatNumber = s.Number,
                SeatType = s.Type.ToString()
            })
            .ToListAsync();

        return ApiResponse<List<ScreenSeatDto>>
            .SuccessResponse(seats, "Seats Retrieved Successfully");
    }

    public async Task<ApiResponse<List<ScreenShowDto>>> GetScreenShowsAsync(int screenId)
    {
        var screenExists = await _context.Screens
            .AnyAsync(s => s.ScreenId == screenId);

        if (!screenExists)
            return ApiResponse<List<ScreenShowDto>>
                .FailureResponse("Screen not found");

        var shows = await _context.Shows
            .Where(s => s.ScreenId == screenId)
            .Select(s => new ScreenShowDto
            {
                ShowId = s.ShowId,
                MovieId = s.MovieId,
                ShowTime = s.ShowDateTime
            })
            .ToListAsync();

        return ApiResponse<List<ScreenShowDto>>
            .SuccessResponse(shows, "Shows Retrieved Successfully");
    }
}