using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Booking;

public class BookingDetailDto
{
    [Required]
    public int BookingId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ShowId { get; set; }

    [Required]
    public DateTime BookingDate { get; set; }

    [Required]
    public decimal TotalAmount { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;

    public List<BookingSeatDto> Seats { get; set; } = new();
}