using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.DTOs.Admin;

namespace P12_Movie_Reservation_System_Backend.Interfaces;

public interface IAdminService
{
    // Dashboard
    Task<ApiResponse<AdminDashboardDto>> GetDashboardAsync();

    // Summary Statistics
    Task<ApiResponse<RevenueDto>> GetRevenueAsync();
    Task<ApiResponse<int>> GetBookingsCountAsync();
    Task<ApiResponse<int>> GetMoviesCountAsync();
    Task<ApiResponse<int>> GetUsersCountAsync();
    Task<ApiResponse<TheaterCountDto>> GetTheaterCountAsync();
    Task<ApiResponse<ScreenCountDto>> GetScreenCountAsync();
    Task<ApiResponse<SeatCountDto>> GetSeatCountAsync();

    // Booking Analytics
    Task<ApiResponse<TodayBookingsDto>> GetTodayBookingsAsync();
    Task<ApiResponse<TodayRevenueDto>> GetTodayRevenueAsync();
    Task<ApiResponse<AverageTicketPriceDto>> GetAverageTicketPriceAsync();
    Task<ApiResponse<OccupancyDto>> GetOccupancyAsync();

    // Show Analytics
    Task<ApiResponse<UpcomingShowsDto>> GetUpcomingShowsAsync();
    Task<ApiResponse<List<PopularMovieDto>>> GetPopularMoviesAsync();

    // Customer Analytics
    Task<ApiResponse<List<TopCustomerDto>>> GetTopCustomersAsync();
    Task<ApiResponse<List<RecentBookingDto>>> GetRecentBookingsAsync();

    // Payment Analytics
    Task<ApiResponse<List<RecentPaymentDto>>> GetRecentPaymentsAsync();
    Task<ApiResponse<PaymentStatisticsDto>> GetPaymentStatisticsAsync();
}