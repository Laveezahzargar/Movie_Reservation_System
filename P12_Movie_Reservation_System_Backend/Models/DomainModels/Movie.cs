using System.ComponentModel.DataAnnotations;
using P12_Movie_Reservation_System_Backend.Models.JunctionModels;

namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class Movie
{
    [Key]
    public int MovieId { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, 600)]
    public int DurationMinutes { get; set; }

    [Required]
    [StringLength(100)]
    public string Genre { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Language { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    [Range(0, 10)]
    public decimal Rating { get; set; }

    [StringLength(20)]
    public string Certificate { get; set; } = "U/A";

    [StringLength(500)]
    public string? PosterUrl { get; set; }

    [StringLength(500)]
    public string? TrailerUrl { get; set; }

    [StringLength(200)]
    public string Director { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; } = true;

    public bool IsFeatured { get; set; } = false;

    // Navigation properties
    public ICollection<Show> Shows { get; set; } = new List<Show>();
    public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
}