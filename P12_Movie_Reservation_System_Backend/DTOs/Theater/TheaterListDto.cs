using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Theater;

public class TheaterListDto
{
    [Required]
    public int TheaterId { get; set; }

    [Required]
    [StringLength(150)]
    public string TheaterName { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Location { get; set; } = string.Empty;
}