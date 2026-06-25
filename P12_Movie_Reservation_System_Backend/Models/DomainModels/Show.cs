namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;



public class Show
{
    public int ShowId { get; set; }

    public DateTime DateTime { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; }

    public int ScreenId { get; set; }
    public Screen Screen { get; set; }

    public ICollection<Booking> Bookings { get; set; }

    public ICollection<ShowSeat> ShowSeats { get; set; }
}