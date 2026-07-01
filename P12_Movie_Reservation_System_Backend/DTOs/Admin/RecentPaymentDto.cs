namespace P12_Movie_Reservation_System_Backend.DTOs.Admin;

public class RecentPaymentDto
{
    public int PaymentId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime PaymentDate { get; set; }
}