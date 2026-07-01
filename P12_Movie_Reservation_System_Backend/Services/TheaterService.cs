using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.DTOs.Theater;
using P12_Movie_Reservation_System_Backend.Interfaces;
using P12_Movie_Reservation_System_Backend.Models.DomainModels;

namespace P12_Movie_Reservation_System_Backend.Services;

public class TheaterService : ITheaterService
{
    private readonly ApplicationDbContext _context;

    public TheaterService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<TheaterListDto>>> GetAllTheatersAsync()
    {
        var theaters = await _context.Theaters.ToListAsync();

        var result = theaters.Select(t => new TheaterListDto
        {
            TheaterId = t.TheaterId,
            TheaterName = t.TheaterName,
            Location = t.Location
        }).ToList();

        return ApiResponse<List<TheaterListDto>>
            .SuccessResponse(result, "Theaters Retrieved Successfully");
    }

    public async Task<ApiResponse<TheaterDetailDto>> CreateTheaterAsync(CreateTheaterDto request)
    {
        var theater = new Theater
        {
            TheaterName = request.TheaterName,
            Location = request.Location
        };

        await _context.Theaters.AddAsync(theater);
        await _context.SaveChangesAsync();

        return ApiResponse<TheaterDetailDto>.SuccessResponse(
            new TheaterDetailDto
            {
                TheaterId = theater.TheaterId,
                TheaterName = theater.TheaterName,
                Location = theater.Location
            },
            "Theater Created Successfully");
    }

    public async Task<ApiResponse<List<TheaterScreenDto>>> GetTheaterScreensAsync(int theaterId)
    {
        var theaterExists = await _context.Theaters
            .AnyAsync(t => t.TheaterId == theaterId);

        if (!theaterExists)
            return ApiResponse<List<TheaterScreenDto>>
                .FailureResponse("Theater not found");

        var screens = await _context.Screens
            .Where(s => s.TheaterId == theaterId)
            .Select(s => new TheaterScreenDto
            {
                ScreenId = s.ScreenId,
                ScreenName = s.ScreenName,
                Capacity = s.Seats.Count(),
            })
            .ToListAsync();

        return ApiResponse<List<TheaterScreenDto>>
            .SuccessResponse(screens, "Screens Retrieved Successfully");
    }
}