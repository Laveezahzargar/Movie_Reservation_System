using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.DTOs.Ticket;

namespace P12_Movie_Reservation_System_Backend.Interfaces;

public interface ITicketService
{
    Task<ApiResponse<TicketDetailDto>> GetTicketByBookingIdAsync(int bookingId);

    Task<byte[]> DownloadTicketAsync(int bookingId);
}