namespace P12_Movie_Reservation_System_Backend.DTOs.Admin;

public class PopularMovieDto
{
    public int MovieId { get; set; }
    public string Title { get; set; }
    public int BookingCount { get; set; }
}