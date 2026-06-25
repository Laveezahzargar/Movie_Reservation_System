
namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class BookingSeat
{
    public int BookingSeatId { get; set; }

    public int BookingId { get; set; }
    public Booking Booking { get; set; }

    public int ShowSeatId { get; set; }
    public ShowSeat ShowSeat { get; set; }
}