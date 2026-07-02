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
    private readonly ILogger<SeatService> _logger;

    public SeatService(ApplicationDbContext context, ILogger<SeatService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResponse<SeatDetailDto>> CreateSeatAsync(CreateSeatDto request)
    {
        _logger.LogInformation(
            "Creating seat {SeatNumber} of type {SeatType} for ScreenId {ScreenId}.",
            request.Number,
            request.Type,
            request.ScreenId);

        var screenExists = await _context.Screens
            .AnyAsync(s => s.ScreenId == request.ScreenId);

        if (!screenExists)
        {
            _logger.LogWarning(
                "Seat creation failed. ScreenId {ScreenId} not found.",
                request.ScreenId);

            return ApiResponse<SeatDetailDto>
                .FailureResponse("Screen not found");
        }

        if (!Enum.TryParse<SeatType>(request.Type, true, out var seatType))
        {
            _logger.LogWarning(
                "Seat creation failed. Invalid seat type '{SeatType}' received.",
                request.Type);

            return ApiResponse<SeatDetailDto>
                .FailureResponse("Invalid seat type");
        }

        var seat = new Seat
        {
            Number = request.Number,
            Type = seatType,
            ScreenId = request.ScreenId
        };

        await _context.Seats.AddAsync(seat);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Seat created successfully. SeatId {SeatId}, SeatNumber {SeatNumber}, ScreenId {ScreenId}.",
            seat.SeatId,
            seat.Number,
            seat.ScreenId);

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