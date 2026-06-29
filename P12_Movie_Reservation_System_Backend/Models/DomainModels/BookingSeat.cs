using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class BookingSeat
{
    [Key]
    public int BookingSeatId { get; set; }

    // Foreign Key
    [Required]
    public int BookingId { get; set; }

    public Booking Booking { get; set; } = null!;

    // Foreign Key
    [Required]
    public int ShowSeatId { get; set; }

    public ShowSeat ShowSeat { get; set; } = null!;
}