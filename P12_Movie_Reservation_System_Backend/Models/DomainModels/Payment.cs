using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P12_Movie_Reservation_System_Backend.Enums;

namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, 999999.99)]
    public decimal Amount { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [Required]
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.UPI;

    [Required]
    [StringLength(100)]
    public string TransactionId { get; set; } = string.Empty;

    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    // Foreign Key
    [Required]
    public int BookingId { get; set; }

    public Booking Booking { get; set; } = null!;
}