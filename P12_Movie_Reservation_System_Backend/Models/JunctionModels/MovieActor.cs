using System.ComponentModel.DataAnnotations;

using P12_Movie_Reservation_System_Backend.Models.DomainModels;

namespace P12_Movie_Reservation_System_Backend.Models.JunctionModels;

public class MovieActor
{
    [Required]
    public int MovieId { get; set; }

    public Movie Movie { get; set; } = null!;

    [Required]
    public int ActorId { get; set; }

    public Actor Actor { get; set; } = null!;
}