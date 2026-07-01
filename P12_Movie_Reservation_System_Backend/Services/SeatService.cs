using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.DTOs.Seat;
using P12_Movie_Reservation_System_Backend.Enums;
using P12_Movie_Reservation_System_Backend.Interfaces;
using P12_Movie_Reservation_System_Backend.Models.DomainModels;

namespace P12_Movie_Reservation_System_Backend.Services;

public class SeatService : ISeatService
{
    private readonly ApplicationDbContext _context;

    public SeatService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<ApiResponse<SeatDetailDto>> CreateSeatAsync(CreateSeatDto request)
    {
        var screenExists = await _context.Screens
            .AnyAsync(s => s.ScreenId == request.ScreenId);

        if (!screenExists)
            return ApiResponse<SeatDetailDto>
                .FailureResponse("Screen not found");

        if (!Enum.TryParse<SeatType>(request.Type, true, out var seatType))
            return ApiResponse<SeatDetailDto>
                .FailureResponse("Invalid seat type");

        var seat = new Seat
        {
            Number = request.Number,
            Type = seatType,
            ScreenId = request.ScreenId
        };

        await _context.Seats.AddAsync(seat);
        await _context.SaveChangesAsync();

        return ApiResponse<SeatDetailDto>.SuccessResponse(
            new SeatDetailDto
            {
                SeatId = seat.SeatId,
                Number = seat.Number,
                Type = seat.Type.ToString(),
                ScreenId = seat.ScreenId
            },
            "Seat Created Successfully");
    }
}