using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Show;

public class ShowDetailDto
{
    public int ShowId { get; set; }

    public int MovieId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    public int TheaterId { get; set; }

    public string TheaterName { get; set; } = string.Empty;

    public int ScreenId { get; set; }

    public string ScreenName { get; set; } = string.Empty;

    public DateTime ShowDateTime { get; set; }
}