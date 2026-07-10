using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Theater;

public class CreateTheaterDto
{
    [Required]
    [StringLength(150, MinimumLength = 2)]
    public string TheaterName { get; set; } = string.Empty;

    [Required]
    [StringLength(255, MinimumLength = 5)]
    public string Location { get; set; } = string.Empty;

    [Required]
    public int CityId { get; set; }
}