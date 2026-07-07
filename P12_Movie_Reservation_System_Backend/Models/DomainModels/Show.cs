using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class Show
{
    [Key]
    public int ShowId { get; set; }

    [Required]
    public DateTime ShowDateTime { get; set; }

    // Foreign Key
    public int? MovieId { get; set; }
    public Movie? Movie { get; set; }

    public int? TheaterId { get; set; }
    public Theater? Theater { get; set; }

    public int? ScreenId { get; set; }
    public Screen? Screen { get; set; }

    // Navigation Properties
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public ICollection<ShowSeat> ShowSeats { get; set; } = new List<ShowSeat>();
}