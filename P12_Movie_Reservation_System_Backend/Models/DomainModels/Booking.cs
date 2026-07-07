using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class Booking
{
    [Key]
    public int BookingId { get; set; }

    [Required]
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, 999999.99)]
    public decimal TotalAmount { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Pending";

    // Foreign Key
    [Required]
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    // Foreign Key
    [Required]
    public int ShowId { get; set; }

    public Show Show { get; set; } = null!;
    public int MovieId { get; set; }
    public Movie Movie { get; set; }

    public int TheaterId { get; set; }
    public Theater Theater { get; set; }

    public int ScreenId { get; set; }
    public Screen Screen { get; set; }

    // One-to-One
    public Payment? Payment { get; set; }

    // One-to-One
    public Ticket? Ticket { get; set; }

    // One-to-Many
    public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
}