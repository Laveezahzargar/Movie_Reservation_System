using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Screen;

public class ScreenShowDto
{
    [Required]
    public int ShowId { get; set; }

    [Required]
    public int MovieId { get; set; }

    [Required]
    public DateTime ShowTime { get; set; }
}