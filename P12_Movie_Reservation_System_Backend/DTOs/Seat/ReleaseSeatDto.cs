using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Seat
{
    public class ReleaseSeatDto
    {
        [Required]
        public int ShowSeatId { get; set; }
    }
}
