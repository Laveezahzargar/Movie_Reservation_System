using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Movie;

public class CreateMovieDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, 600)]
    public int Duration { get; set; }

    [Required]
    [StringLength(100)]
    public string Genre { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Language { get; set; } = string.Empty;

    [Required]
    public DateTime ReleaseDate { get; set; }

    [Range(0, 10)]
    public decimal Rating { get; set; }

    [Required]
    [StringLength(20)]
    public string Certificate { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Director { get; set; } = string.Empty;

    [Url]
    public string? PosterUrl { get; set; }

    [Url]
    public string? TrailerUrl { get; set; }

    public bool IsFeatured { get; set; }
}