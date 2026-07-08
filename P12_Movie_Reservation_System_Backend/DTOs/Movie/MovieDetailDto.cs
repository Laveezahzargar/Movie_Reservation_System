namespace P12_Movie_Reservation_System_Backend.DTOs.Movie;

public class MovieDetailDto
{
    public int MovieId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Duration { get; set; }

    public string Genre { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public DateTime ReleaseDate { get; set; }

    public decimal Rating { get; set; }

    public string Certificate { get; set; } = string.Empty;

    public string Director { get; set; } = string.Empty;

    public string? PosterUrl { get; set; }

    public string? TrailerUrl { get; set; }

    public bool IsFeatured { get; set; }
}