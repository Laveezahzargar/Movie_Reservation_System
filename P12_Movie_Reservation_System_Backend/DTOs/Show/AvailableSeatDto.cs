using System.ComponentModel.DataAnnotations;
using P12_Movie_Reservation_System_Backend.Enums;

namespace P12_Movie_Reservation_System_Backend.DTOs.Show;

public class AvailableSeatDto
{
    public int ShowSeatId { get; set; }

    public int SeatId { get; set; }

    public string SeatNumber { get; set; } = string.Empty;

    public string SeatType { get; set; } = string.Empty;

    public ShowSeatStatus Status { get; set; } = ShowSeatStatus.Available;
}