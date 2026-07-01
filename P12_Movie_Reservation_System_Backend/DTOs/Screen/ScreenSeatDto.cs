using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Screen;

public class ScreenSeatDto
{
    [Required]
    public int SeatId { get; set; }

    [Required]
    [StringLength(20)]
    public string SeatNumber { get; set; } = string.Empty;

    [Required]
    public string SeatType { get; set; } = string.Empty;
}