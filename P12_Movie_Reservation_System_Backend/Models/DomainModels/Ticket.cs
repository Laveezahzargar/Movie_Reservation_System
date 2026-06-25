namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;


public class Ticket
{
    public int TicketId { get; set; }

    public string Number { get; set; }

    public DateTime GeneratedAt { get; set; }

    public string QRCodeUrl { get; set; }

    public int BookingId { get; set; }
    public Booking Booking { get; set; }
}