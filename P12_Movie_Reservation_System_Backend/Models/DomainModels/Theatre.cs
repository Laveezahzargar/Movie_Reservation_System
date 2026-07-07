using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class Theater
{
    [Key]
    public int TheaterId { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 2)]
    public string TheaterName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public int CityId { get; set; }

    public City City { get; set; } = null!;

    // Navigation Property
    public ICollection<Screen> Screens { get; set; } = new List<Screen>();
}