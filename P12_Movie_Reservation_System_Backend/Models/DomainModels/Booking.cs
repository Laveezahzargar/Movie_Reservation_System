namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class Booking
{
    public int BookingId { get; set; }

    public DateTime BookingDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int ShowId { get; set; }
    public Show Show { get; set; }

    public Payment Payment { get; set; }
    public Ticket Ticket { get; set; }
    public ICollection<BookingSeat> BookingSeats { get; set; }
}