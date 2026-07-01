using P12_Movie_Reservation_System_Backend.Enums;

namespace P12_Movie_Reservation_System_Backend.DTOs.Payment;

public class PaymentDetailDto
{
    public int PaymentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string TransactionId { get; set; }
    public PaymentStatus Status { get; set; }
    public int BookingId { get; set; }
}