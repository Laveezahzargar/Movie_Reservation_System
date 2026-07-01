using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Screen;

public class ScreenListDto
{
    [Required]
    public int ScreenId { get; set; }

    [Required]
    [StringLength(50)]
    public string ScreenName { get; set; } = string.Empty;

    [Required]
    public int TheaterId { get; set; }
}