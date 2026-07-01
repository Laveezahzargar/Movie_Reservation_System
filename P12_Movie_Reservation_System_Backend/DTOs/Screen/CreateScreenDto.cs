using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Screen;

public class CreateScreenDto
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string ScreenName { get; set; } = string.Empty;

    [Required]
    public int TheaterId { get; set; }
}