using P12_Movie_Reservation_System_Backend.Models.JunctionModels;

namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;


public class Actor
{
    public int ActorId { get; set; }

    public string ActorName { get; set; }

    public ICollection<MovieActor> MovieActors { get; set; }
}