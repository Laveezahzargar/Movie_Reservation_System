using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Show;

public class AvailableSeatDto
{
    [Required]
    public int SeatId { get; set; }

    [Required]
    public string SeatNumber { get; set; } = string.Empty;

    [Required]
    public string SeatType { get; set; } = string.Empty;
}