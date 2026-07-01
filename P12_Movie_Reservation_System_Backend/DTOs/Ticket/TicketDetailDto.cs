namespace P12_Movie_Reservation_System_Backend.DTOs.Ticket;

public class TicketDetailDto
{
    public int TicketId { get; set; }
    public string TicketNumber { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string QRCodePath { get; set; }
    public int BookingId { get; set; }
}