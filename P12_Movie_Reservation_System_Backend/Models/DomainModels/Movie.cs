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
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, 600)]
    public int DurationMinutes { get; set; }

    [Required]
    [StringLength(100)]
    public string Genre { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    public ICollection<Show> Shows { get; set; } = new List<Show>();

    public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
}