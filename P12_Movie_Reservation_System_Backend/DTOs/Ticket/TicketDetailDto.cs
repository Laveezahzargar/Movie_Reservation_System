namespace P12_Movie_Reservation_System_Backend.DTOs.Ticket;

public class TicketDetailDto
{
    public int TicketId { get; set; }
    public string TicketNumber { get; set; } = "";
    public DateTime GeneratedAt { get; set; }
    public string QRCodePath { get; set; } = "";
    public int BookingId { get; set; }

    // 🔥 UI fields
    public string MovieTitle { get; set; } = "";
    public string TheaterName { get; set; } = "";
    public string ScreenName { get; set; } = "";
    public string ShowDate { get; set; } = "";
    public string ShowTime { get; set; } = "";
    public string Seats { get; set; } = "";
    public decimal Amount { get; set; }
}