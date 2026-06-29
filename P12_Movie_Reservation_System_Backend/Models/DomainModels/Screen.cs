using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class Screen
{
    [Key]
    public int ScreenId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string ScreenName { get; set; } = string.Empty;

    // Foreign Key
    [Required]
    public int TheaterId { get; set; }

    public Theater Theater { get; set; } = null!;

    // Navigation Properties
    public ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public ICollection<Show> Shows { get; set; } = new List<Show>();
}