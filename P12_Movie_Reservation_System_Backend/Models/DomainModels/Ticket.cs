using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class Ticket
{
    [Key]
    public int TicketId { get; set; }

    [Required]
    [StringLength(50)]
    public string TicketNumber { get; set; } = string.Empty;

    [Required]
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [StringLength(500)]
    public string QRCodePath { get; set; } = string.Empty;

    // Foreign Key
    [Required]
    public int BookingId { get; set; }

    public Booking Booking { get; set; } = null!;
}