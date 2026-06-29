using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class Theater
{
    [Key]
    public int TheaterId { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 2)]
    public string TheaterName { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Location { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<Screen> Screens { get; set; } = new List<Screen>();
}