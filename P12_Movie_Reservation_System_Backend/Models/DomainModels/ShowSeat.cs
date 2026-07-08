using System.ComponentModel.DataAnnotations;
using P12_Movie_Reservation_System_Backend.Enums;

namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class ShowSeat
{
    [Key]
    public int ShowSeatId { get; set; }

    // Foreign Key
    [Required]
    public int ShowId { get; set; }

    public Show Show { get; set; } = null!;

    // Foreign Key
    [Required]
    public int SeatId { get; set; }

    public Seat Seat { get; set; } = null!;

    [Required]
    public ShowSeatStatus Status { get; set; } = ShowSeatStatus.Available;
    public int? ReservedByUserId { get; set; }

    public User? ReservedByUser { get; set; }

    public DateTime? ReservedUntil { get; set; }

    // Navigation Property
    public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
}