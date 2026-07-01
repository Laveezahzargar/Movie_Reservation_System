using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P12_Movie_Reservation_System_Backend.Interfaces;

namespace P12_Movie_Reservation_System_Backend.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
        => Ok(await _adminService.GetDashboardAsync());

    [HttpGet("revenue")]
    public async Task<IActionResult> Revenue()
        => Ok(await _adminService.GetRevenueAsync());

    [HttpGet("revenue/today")]
    public async Task<IActionResult> TodayRevenue()
        => Ok(await _adminService.GetTodayRevenueAsync());

    [HttpGet("bookings/count")]
    public async Task<IActionResult> BookingsCount()
        => Ok(await _adminService.GetBookingsCountAsync());

    [HttpGet("movies/count")]
    public async Task<IActionResult> MoviesCount()
        => Ok(await _adminService.GetMoviesCountAsync());

    [HttpGet("users/count")]
    public async Task<IActionResult> UsersCount()
        => Ok(await _adminService.GetUsersCountAsync());

    [HttpGet("theaters/count")]
    public async Task<IActionResult> TheatersCount()
        => Ok(await _adminService.GetTheaterCountAsync());

    [HttpGet("screens/count")]
    public async Task<IActionResult> ScreensCount()
        => Ok(await _adminService.GetScreenCountAsync());

    [HttpGet("seats/count")]
    public async Task<IActionResult> SeatsCount()
        => Ok(await _adminService.GetSeatCountAsync());

    [HttpGet("bookings/today")]
    public async Task<IActionResult> TodayBookings()
        => Ok(await _adminService.GetTodayBookingsAsync());

    [HttpGet("recent-bookings")]
    public async Task<IActionResult> RecentBookings()
        => Ok(await _adminService.GetRecentBookingsAsync());

    [HttpGet("popular-movies")]
    public async Task<IActionResult> PopularMovies()
        => Ok(await _adminService.GetPopularMoviesAsync());

    [HttpGet("upcoming-shows")]
    public async Task<IActionResult> UpcomingShows()
        => Ok(await _adminService.GetUpcomingShowsAsync());

    [HttpGet("average-ticket-price")]
    public async Task<IActionResult> AverageTicketPrice()
        => Ok(await _adminService.GetAverageTicketPriceAsync());

    [HttpGet("occupancy")]
    public async Task<IActionResult> Occupancy()
        => Ok(await _adminService.GetOccupancyAsync());

    [HttpGet("top-customers")]
    public async Task<IActionResult> TopCustomers()
        => Ok(await _adminService.GetTopCustomersAsync());

    [HttpGet("recent-payments")]
    public async Task<IActionResult> RecentPayments()
        => Ok(await _adminService.GetRecentPaymentsAsync());

    [HttpGet("payment-statistics")]
    public async Task<IActionResult> PaymentStatistics()
        => Ok(await _adminService.GetPaymentStatisticsAsync());
}