using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.DTOs.Admin;
using P12_Movie_Reservation_System_Backend.Enums;
using P12_Movie_Reservation_System_Backend.Interfaces;

namespace P12_Movie_Reservation_System_Backend.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _context;

    public AdminService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ============================================================
    // Dashboard
    // ============================================================

    public async Task<ApiResponse<AdminDashboardDto>> GetDashboardAsync()
    {
        var users = await _context.Users.CountAsync();

        var movies = await _context.Movies.CountAsync();

        var bookings = await _context.Bookings.CountAsync();

        var revenue = await _context.Payments
            .Where(p => p.Status == PaymentStatus.Success)
            .SumAsync(p => p.Amount);

        var recentBookings = await GetRecentBookingsAsync();

        var popularMovies = await GetPopularMoviesAsync();

        var dashboard = new AdminDashboardDto
        {
            TotalUsers = users,
            TotalMovies = movies,
            TotalBookings = bookings,
            TotalRevenue = revenue,
            RecentBookings = recentBookings.Data,
            PopularMovies = popularMovies.Data
        };

        return ApiResponse<AdminDashboardDto>.SuccessResponse(
            dashboard,
            "Dashboard loaded successfully");
    }

    // ============================================================
    // Revenue
    // ============================================================

    public async Task<ApiResponse<RevenueDto>> GetRevenueAsync()
    {
        var revenue = await _context.Payments
            .Where(p => p.Status == PaymentStatus.Success)
            .SumAsync(p => p.Amount);

        var dto = new RevenueDto
        {
            TotalRevenue = revenue
        };

        return ApiResponse<RevenueDto>.SuccessResponse(
            dto,
            "Revenue retrieved successfully");
    }

    // ============================================================
    // Bookings Count
    // ============================================================

    public async Task<ApiResponse<int>> GetBookingsCountAsync()
    {
        var count = await _context.Bookings.CountAsync();

        return ApiResponse<int>.SuccessResponse(
            count,
            "Bookings count retrieved successfully");
    }

    // ============================================================
    // Movies Count
    // ============================================================

    public async Task<ApiResponse<int>> GetMoviesCountAsync()
    {
        var count = await _context.Movies.CountAsync();

        return ApiResponse<int>.SuccessResponse(
            count,
            "Movies count retrieved successfully");
    }

    // ============================================================
    // Users Count
    // ============================================================

    public async Task<ApiResponse<int>> GetUsersCountAsync()
    {
        var count = await _context.Users.CountAsync();

        return ApiResponse<int>.SuccessResponse(
            count,
            "Users count retrieved successfully");
    }

    // ============================================================
    // Theaters Count
    // ============================================================

    public async Task<ApiResponse<TheaterCountDto>> GetTheaterCountAsync()
    {
        var dto = new TheaterCountDto
        {
            TotalTheaters = await _context.Theaters.CountAsync()
        };

        return ApiResponse<TheaterCountDto>.SuccessResponse(
            dto,
            "Theaters count retrieved successfully");
    }

    // ============================================================
    // Screens Count
    // ============================================================

    public async Task<ApiResponse<ScreenCountDto>> GetScreenCountAsync()
    {
        var dto = new ScreenCountDto
        {
            TotalScreens = await _context.Screens.CountAsync()
        };

        return ApiResponse<ScreenCountDto>.SuccessResponse(
            dto,
            "Screens count retrieved successfully");
    }

    // ============================================================
    // Seats Count
    // ============================================================

    public async Task<ApiResponse<SeatCountDto>> GetSeatCountAsync()
    {
        var dto = new SeatCountDto
        {
            TotalSeats = await _context.Seats.CountAsync()
        };

        return ApiResponse<SeatCountDto>.SuccessResponse(
            dto,
            "Seats count retrieved successfully");
    }
    // ============================================================
    // Today's Bookings
    // ============================================================

    public async Task<ApiResponse<TodayBookingsDto>> GetTodayBookingsAsync()
    {
        var count = await _context.Bookings
            .CountAsync(b => b.BookingDate.Date == DateTime.UtcNow.Date);

        var dto = new TodayBookingsDto
        {
            TotalBookingsToday = count
        };

        return ApiResponse<TodayBookingsDto>.SuccessResponse(
            dto,
            "Today's bookings retrieved successfully");
    }

    // ============================================================
    // Today's Revenue
    // ============================================================

    public async Task<ApiResponse<TodayRevenueDto>> GetTodayRevenueAsync()
    {
        var revenue = await _context.Payments
            .Where(p =>
                p.Status == PaymentStatus.Success &&
                p.PaymentDate.Date == DateTime.UtcNow.Date)
            .SumAsync(p => p.Amount);

        var dto = new TodayRevenueDto
        {
            RevenueToday = revenue
        };

        return ApiResponse<TodayRevenueDto>.SuccessResponse(
            dto,
            "Today's revenue retrieved successfully");
    }

    // ============================================================
    // Upcoming Shows
    // ============================================================

    public async Task<ApiResponse<UpcomingShowsDto>> GetUpcomingShowsAsync()
    {
        var count = await _context.Shows
            .CountAsync(s => s.ShowDateTime > DateTime.UtcNow);

        var dto = new UpcomingShowsDto
        {
            UpcomingShows = count
        };

        return ApiResponse<UpcomingShowsDto>.SuccessResponse(
            dto,
            "Upcoming shows retrieved successfully");
    }

    // ============================================================
    // Average Ticket Price
    // ============================================================

    public async Task<ApiResponse<AverageTicketPriceDto>> GetAverageTicketPriceAsync()
    {
        decimal average = 0;

        if (await _context.Bookings.AnyAsync())
        {
            average = await _context.Bookings
                .AverageAsync(b => b.TotalAmount);
        }

        var dto = new AverageTicketPriceDto
        {
            AverageTicketPrice = average
        };

        return ApiResponse<AverageTicketPriceDto>.SuccessResponse(
            dto,
            "Average ticket price retrieved successfully");
    }

    // ============================================================
    // Occupancy
    // ============================================================

    public async Task<ApiResponse<OccupancyDto>> GetOccupancyAsync()
    {
        var totalSeats = await _context.ShowSeats.CountAsync();

        var bookedSeats = await _context.ShowSeats
            .CountAsync(s => s.Status == ShowSeatStatus.Booked);

        double occupancy = 0;

        if (totalSeats > 0)
        {
            occupancy = (double)bookedSeats / totalSeats * 100;
        }

        var dto = new OccupancyDto
        {
            TotalSeats = totalSeats,
            BookedSeats = bookedSeats,
            OccupancyPercentage = Math.Round(occupancy, 2)
        };

        return ApiResponse<OccupancyDto>.SuccessResponse(
            dto,
            "Occupancy retrieved successfully");
    }

    // ============================================================
    // Recent Bookings
    // ============================================================

    public async Task<ApiResponse<List<RecentBookingDto>>> GetRecentBookingsAsync()
    {
        var bookings = await _context.Bookings
            .OrderByDescending(b => b.BookingDate)
            .Take(5)
            .Select(b => new RecentBookingDto
            {
                BookingId = b.BookingId,
                UserName = b.User.Name,
                MovieTitle = b.Show.Movie.Title,
                TotalAmount = b.TotalAmount,
                BookingDate = b.BookingDate
            })
            .ToListAsync();

        return ApiResponse<List<RecentBookingDto>>.SuccessResponse(
            bookings,
            "Recent bookings retrieved successfully");
    }

    // ============================================================
    // Popular Movies
    // ============================================================

    public async Task<ApiResponse<List<PopularMovieDto>>> GetPopularMoviesAsync()
    {
        var movies = await _context.Bookings
            .GroupBy(b => new
            {
                b.Show.MovieId,
                b.Show.Movie.Title
            })
            .Select(g => new PopularMovieDto
            {
                MovieId = g.Key.MovieId,
                Title = g.Key.Title,
                BookingCount = g.Count()
            })
            .OrderByDescending(x => x.BookingCount)
            .Take(5)
            .ToListAsync();

        return ApiResponse<List<PopularMovieDto>>.SuccessResponse(
            movies,
            "Popular movies retrieved successfully");
    }

    // ============================================================
    // Top Customers
    // ============================================================

    public async Task<ApiResponse<List<TopCustomerDto>>> GetTopCustomersAsync()
    {
        var customers = await _context.Bookings
            .GroupBy(b => new
            {
                b.UserId,
                b.User.Name
            })
            .Select(g => new TopCustomerDto
            {
                UserId = g.Key.UserId,
                UserName = g.Key.Name,
                TotalBookings = g.Count(),
                TotalSpent = g.Sum(x => x.TotalAmount)
            })
            .OrderByDescending(x => x.TotalSpent)
            .Take(5)
            .ToListAsync();

        return ApiResponse<List<TopCustomerDto>>.SuccessResponse(
            customers,
            "Top customers retrieved successfully");
    }

    // ============================================================
    // Recent Payments
    // ============================================================

    public async Task<ApiResponse<List<RecentPaymentDto>>> GetRecentPaymentsAsync()
    {
        var payments = await _context.Payments
            .OrderByDescending(p => p.PaymentDate)
            .Take(5)
            .Select(p => new RecentPaymentDto
            {
                PaymentId = p.PaymentId,
                UserName = p.Booking.User.Name,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod.ToString(),
                Status = p.Status.ToString(),
                PaymentDate = p.PaymentDate
            })
            .ToListAsync();

        return ApiResponse<List<RecentPaymentDto>>.SuccessResponse(
            payments,
            "Recent payments retrieved successfully");
    }

    // ============================================================
    // Payment Statistics
    // ============================================================

    public async Task<ApiResponse<PaymentStatisticsDto>> GetPaymentStatisticsAsync()
    {
        var dto = new PaymentStatisticsDto
        {
            SuccessfulPayments = await _context.Payments
                .CountAsync(p => p.Status == PaymentStatus.Success),

            PendingPayments = await _context.Payments
                .CountAsync(p => p.Status == PaymentStatus.Pending),

            FailedPayments = await _context.Payments
                .CountAsync(p => p.Status == PaymentStatus.Failed)
        };

        return ApiResponse<PaymentStatisticsDto>.SuccessResponse(
            dto,
            "Payment statistics retrieved successfully");
    }
}
