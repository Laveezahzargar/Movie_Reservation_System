using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Show;

public class ShowDetailDto
{
    [Required]
    public int ShowId { get; set; }

    [Required]
    public int MovieId { get; set; }

    [Required]
    public int ScreenId { get; set; }

    [Required]
    public DateTime ShowDateTime { get; set; }
}