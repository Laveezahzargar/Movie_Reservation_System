namespace P12_Movie_Reservation_System_Backend.DTOs.Admin;

public class OccupancyDto
{
    public int TotalSeats { get; set; }

    public int BookedSeats { get; set; }

    public double OccupancyPercentage { get; set; }
}