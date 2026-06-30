using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Movie;

public class UpdateMovieDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Genre { get; set; } = string.Empty;

    [Required]
    [Range(1, 600)]
    public int Duration { get; set; }

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime ReleaseDate { get; set; }
}