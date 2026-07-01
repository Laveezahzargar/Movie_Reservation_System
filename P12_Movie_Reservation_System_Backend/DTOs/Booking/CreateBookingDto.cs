using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Booking;

public class CreateBookingDto
{
    [Required]
    public int ShowId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one seat must be selected.")]
    public List<int> ShowSeatIds { get; set; } = new();
}