using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Seat;

public class CreateSeatDto
{
    [Required]
    [StringLength(10, MinimumLength = 1)]
    public string Number { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = "Regular";

    [Required]
    public int ScreenId { get; set; }
}