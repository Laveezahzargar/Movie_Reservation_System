




namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;


public class Screen
{
    public int ScreenId { get; set; }

    public string ScreenName { get; set; }

    public int TheaterId { get; set; }
    public Theater Theater { get; set; }

    public ICollection<Seat> Seats { get; set; }

    public ICollection<Show> Shows { get; set; }
}