namespace P12_Movie_Reservation_System_Backend.DTOs.Admin;

public class PaymentStatisticsDto
{
    public int SuccessfulPayments { get; set; }

    public int PendingPayments { get; set; }

    public int FailedPayments { get; set; }
}