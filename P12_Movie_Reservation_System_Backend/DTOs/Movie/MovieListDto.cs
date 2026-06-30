namespace P12_Movie_Reservation_System_Backend.DTOs.Movie;

public class MovieListDto
{
    public int MovieId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
}