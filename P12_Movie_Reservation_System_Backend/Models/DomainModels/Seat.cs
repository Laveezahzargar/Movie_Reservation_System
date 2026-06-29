using System.ComponentModel.DataAnnotations;
using P12_Movie_Reservation_System_Backend.Enums;


namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class Seat
{
    [Key]
    public int SeatId { get; set; }

    [Required]
    [StringLength(10, MinimumLength = 1)]
    public string Number { get; set; } = string.Empty;

    [Required]
    public SeatType Type { get; set; } = SeatType.Regular;

    // Foreign Key
    [Required]
    public int ScreenId { get; set; }

    public Screen Screen { get; set; } = null!;

    // Navigation Property
    public ICollection<ShowSeat> ShowSeats { get; set; } = new List<ShowSeat>();
}