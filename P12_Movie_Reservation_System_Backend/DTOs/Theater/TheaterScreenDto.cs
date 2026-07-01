using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Theater;

public class TheaterScreenDto
{
    [Required]
    public int ScreenId { get; set; }

    [Required]
    [StringLength(100)]
    public string ScreenName { get; set; } = string.Empty;

    [Required]
    [Range(1, 1000)]
    public int Capacity { get; set; }
}