using System.ComponentModel.DataAnnotations;
using P12_Movie_Reservation_System_Backend.Models.JunctionModels;

namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class Actor
{
    [Key]
    public int ActorId { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string ActorName { get; set; } = string.Empty;

    public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
}