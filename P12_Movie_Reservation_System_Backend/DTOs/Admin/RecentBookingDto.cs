namespace P12_Movie_Reservation_System_Backend.DTOs.Admin;

public class RecentBookingDto
{
    public int BookingId { get; set; }
    public string UserName { get; set; }
    public string MovieTitle { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime BookingDate { get; set; }
}