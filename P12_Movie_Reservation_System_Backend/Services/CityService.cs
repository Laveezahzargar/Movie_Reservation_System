using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.Data;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.DTOs.City;
using P12_Movie_Reservation_System_Backend.Interfaces;
using P12_Movie_Reservation_System_Backend.Models;
using P12_Movie_Reservation_System_Backend.Models.DomainModels;

namespace P12_Movie_Reservation_System_Backend.Services;

public class CityService : ICityService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CityService> _logger;

    public CityService(
        ApplicationDbContext context,
        ILogger<CityService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResponse<List<CityDto>>> GetAllCitiesAsync()
    {
        var cities = await _context.Cities
            .OrderBy(c => c.Name)
            .Select(c => new CityDto
            {
                CityId = c.CityId,
                Name = c.Name,
                State = c.State,
                Country = c.Country
            })
            .ToListAsync();

        return ApiResponse<List<CityDto>>.SuccessResponse(
            cities,
            "Cities retrieved successfully.");
    }

    public async Task<ApiResponse<CityDto>> GetCityByIdAsync(int cityId)
    {
        var city = await _context.Cities
            .Where(c => c.CityId == cityId)
            .Select(c => new CityDto
            {
                CityId = c.CityId,
                Name = c.Name,
                State = c.State,
                Country = c.Country
            })
            .FirstOrDefaultAsync();

        if (city == null)
        {
            return ApiResponse<CityDto>.FailureResponse("City not found.");
        }

        return ApiResponse<CityDto>.SuccessResponse(
            city,
            "City retrieved successfully.");
    }

    public async Task<ApiResponse<CityDto>> CreateCityAsync(CreateCityDto dto)
    {
        bool exists = await _context.Cities
            .AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower());

        if (exists)
        {
            return ApiResponse<CityDto>.FailureResponse("City already exists.");
        }

        var city = new City
        {
            Name = dto.Name,
            State = dto.State,
            Country = dto.Country
        };

        _context.Cities.Add(city);
        await _context.SaveChangesAsync();

        return ApiResponse<CityDto>.SuccessResponse(
            new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State,
                Country = city.Country
            },
            "City created successfully.");
    }

    public async Task<ApiResponse<string>> UpdateCityAsync(int cityId, UpdateCityDto dto)
    {
        var city = await _context.Cities.FindAsync(cityId);

        if (city == null)
        {
            return ApiResponse<string>.FailureResponse("City not found.");
        }

        bool exists = await _context.Cities.AnyAsync(c =>
            c.CityId != cityId &&
            c.Name.ToLower() == dto.Name.ToLower());

        if (exists)
        {
            return ApiResponse<string>.FailureResponse("Another city with the same name already exists.");
        }

        city.Name = dto.Name;
        city.State = dto.State;
        city.Country = dto.Country;

        await _context.SaveChangesAsync();

        return ApiResponse<string>.SuccessResponse("Updated",
            "City updated successfully.");
    }

    public async Task<ApiResponse<string>> DeleteCityAsync(int cityId)
    {
        var city = await _context.Cities
            .Include(c => c.Theaters)
            .FirstOrDefaultAsync(c => c.CityId == cityId);

        if (city == null)
        {
            return ApiResponse<string>.FailureResponse("City not found.");
        }

        if (city.Theaters.Any())
        {
            return ApiResponse<string>.FailureResponse(
                "Cannot delete a city that has theaters.");
        }

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();

        return ApiResponse<string>.SuccessResponse("Deleted",
            "City deleted successfully.");
    }
}