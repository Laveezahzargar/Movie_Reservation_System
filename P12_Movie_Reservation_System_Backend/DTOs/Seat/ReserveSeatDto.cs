using System.ComponentModel.DataAnnotations;

namespace P12_Movie_Reservation_System_Backend.DTOs.Seat
{
    public class ReserveSeatDto
    {
        [Required]
        public int ShowSeatId { get; set; }
    }
}
