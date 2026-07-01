namespace P12_Movie_Reservation_System_Backend.DTOs.Admin;

public class TopCustomerDto
{
    public int UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public int TotalBookings { get; set; }

    public decimal TotalSpent { get; set; }
}