namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;


public class Theater
{
    public int TheaterId { get; set; }

    public string TheaterName { get; set; }

    public string Location { get; set; }

    public ICollection<Screen> Screens { get; set; }
}