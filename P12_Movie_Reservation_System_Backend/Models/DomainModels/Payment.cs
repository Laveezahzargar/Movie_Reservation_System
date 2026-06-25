
namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class Payment
{
    public int PaymentId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentMethod { get; set; } // UPI, Card, NetBanking

    public string TransactionId { get; set; }

    public string Status { get; set; } // Pending, Success, Failed

    public int BookingId { get; set; }
    public Booking Booking { get; set; }
}