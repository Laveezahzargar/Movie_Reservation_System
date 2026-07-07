using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.DTOs.City;
using P12_Movie_Reservation_System_Backend.Models;

namespace P12_Movie_Reservation_System_Backend.Interfaces;

public interface ICityService
{
    Task<ApiResponse<List<CityDto>>> GetAllCitiesAsync();

    Task<ApiResponse<CityDto>> GetCityByIdAsync(int cityId);

    Task<ApiResponse<CityDto>> CreateCityAsync(CreateCityDto dto);

    Task<ApiResponse<string>> UpdateCityAsync(int cityId, UpdateCityDto dto);

    Task<ApiResponse<string>> DeleteCityAsync(int cityId);
}