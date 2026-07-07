using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Show;

public class ShowListDto
{
    public int ShowId { get; set; }

    public int MovieId { get; set; }

    public string MovieTitle { get; set; } = "";

    public int TheaterId { get; set; }

    public string TheaterName { get; set; } = "";

    public int ScreenId { get; set; }

    public string ScreenName { get; set; } = "";

    public DateTime ShowDateTime { get; set; }
}