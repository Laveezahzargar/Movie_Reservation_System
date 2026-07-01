using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Actor;

public class ActorDetailDto
{
    [Required]
    public int ActorId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
}