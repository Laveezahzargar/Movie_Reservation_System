using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Seat;

public class SeatDetailDto
{
    [Required]
    public int SeatId { get; set; }

    [Required]
    public string Number { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    [Required]
    public int ScreenId { get; set; }
}