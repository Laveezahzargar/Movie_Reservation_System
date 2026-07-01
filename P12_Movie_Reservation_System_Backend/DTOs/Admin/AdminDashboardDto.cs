namespace P12_Movie_Reservation_System_Backend.DTOs.Admin;

public class AdminDashboardDto
{
    public int TotalUsers { get; set; }
    public int TotalMovies { get; set; }
    public int TotalBookings { get; set; }
    public decimal TotalRevenue { get; set; }

    public List<RecentBookingDto> RecentBookings { get; set; } = new();
    public List<PopularMovieDto> PopularMovies { get; set; } = new();
}