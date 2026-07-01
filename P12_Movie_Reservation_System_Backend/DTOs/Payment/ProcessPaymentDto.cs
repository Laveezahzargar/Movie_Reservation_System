using System.ComponentModel.DataAnnotations;
using P12_Movie_Reservation_System_Backend.Enums;

namespace P12_Movie_Reservation_System_Backend.DTOs.Payment;

public class ProcessPaymentDto
{
    [Required]
    public int BookingId { get; set; }

    [Required]
    [Range(0.01, 999999.99)]
    public decimal Amount { get; set; }

    [Required]
    public PaymentMethod PaymentMethod { get; set; }
}