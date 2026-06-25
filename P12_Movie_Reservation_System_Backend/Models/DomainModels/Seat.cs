namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;



public class Seat
{
    public int SeatId { get; set; }

    public string Number { get; set; }

    public string Type { get; set; }

    public int ScreenId { get; set; }
    public Screen Screen { get; set; }

    public ICollection<ShowSeat> ShowSeats { get; set; }
}