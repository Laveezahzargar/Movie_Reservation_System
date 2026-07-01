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

    public ShowService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<ShowListDto>>> GetAllShowsAsync()
    {
        var shows = await _context.Shows.ToListAsync();

        var result = shows.Select(s => new ShowListDto
        {
            ShowId = s.ShowId,
            MovieId = s.MovieId,
            ScreenId = s.ScreenId,
            ShowDateTime = s.ShowDateTime
        }).ToList();

        return ApiResponse<List<ShowListDto>>
            .SuccessResponse(result, "Shows Retrieved Successfully");
    }

    public async Task<ApiResponse<ShowDetailDto>> CreateShowAsync(CreateShowDto request)
    {
        var movieExists = await _context.Movies
            .AnyAsync(m => m.MovieId == request.MovieId);

        if (!movieExists)
            return ApiResponse<ShowDetailDto>
                .FailureResponse("Movie not found");

        var screenExists = await _context.Screens
            .AnyAsync(s => s.ScreenId == request.ScreenId);

        if (!screenExists)
            return ApiResponse<ShowDetailDto>
                .FailureResponse("Screen not found");

        var show = new Show
        {
            MovieId = request.MovieId,
            ScreenId = request.ScreenId,
            ShowDateTime = request.ShowDateTime
        };

        await _context.Shows.AddAsync(show);
        await _context.SaveChangesAsync();

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

        return ApiResponse<ShowDetailDto>.SuccessResponse(
            new ShowDetailDto
            {
                ShowId = show.ShowId,
                MovieId = show.MovieId,
                ScreenId = show.ScreenId,
                ShowDateTime = show.ShowDateTime
            },
            "Show Created Successfully");
    }

    public async Task<ApiResponse<List<AvailableSeatDto>>> GetAvailableSeatsAsync(int showId)
    {
        var showExists = await _context.Shows
            .AnyAsync(s => s.ShowId == showId);

        if (!showExists)
            return ApiResponse<List<AvailableSeatDto>>
                .FailureResponse("Show not found");

        var seats = await _context.ShowSeats
            .Where(ss => ss.ShowId == showId && ss.Status == ShowSeatStatus.Available)
            .Select(ss => new AvailableSeatDto
            {
                SeatId = ss.Seat.SeatId,
                SeatNumber = ss.Seat.Number,
                SeatType = ss.Seat.Type.ToString()
            })
            .ToListAsync();

        return ApiResponse<List<AvailableSeatDto>>
            .SuccessResponse(seats, "Available Seats Retrieved Successfully");
    }
}