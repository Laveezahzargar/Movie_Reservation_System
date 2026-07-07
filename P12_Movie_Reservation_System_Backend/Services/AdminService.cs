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
    private readonly ILogger<AdminService> _logger;

    public AdminService(
        ApplicationDbContext context,
        ILogger<AdminService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ============================================================
    // Dashboard
    // ============================================================

    public async Task<ApiResponse<AdminDashboardDto>> GetDashboardAsync()
    {
        _logger.LogInformation("Loading admin dashboard.");

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

        _logger.LogInformation(
            "Admin dashboard loaded successfully. Users: {Users}, Movies: {Movies}, Bookings: {Bookings}, Revenue: {Revenue}.",
            users,
            movies,
            bookings,
            revenue);

        return ApiResponse<AdminDashboardDto>.SuccessResponse(
            dashboard,
            "Dashboard loaded successfully");
    }

    // ============================================================
    // Revenue
    // ============================================================

    public async Task<ApiResponse<RevenueDto>> GetRevenueAsync()
    {
        _logger.LogInformation("Fetching total revenue.");

        var revenue = await _context.Payments
            .Where(p => p.Status == PaymentStatus.Success)
            .SumAsync(p => p.Amount);

        var dto = new RevenueDto
        {
            TotalRevenue = revenue
        };

        _logger.LogInformation(
            "Total revenue retrieved successfully. Revenue: {Revenue}.",
            revenue);

        return ApiResponse<RevenueDto>.SuccessResponse(
            dto,
            "Revenue retrieved successfully");
    }

    // ============================================================
    // Bookings Count
    // ============================================================

    public async Task<ApiResponse<int>> GetBookingsCountAsync()
    {
        _logger.LogInformation("Fetching total bookings count.");

        var count = await _context.Bookings.CountAsync();

        _logger.LogInformation(
            "Bookings count retrieved successfully. Count: {BookingCount}.",
            count);

        return ApiResponse<int>.SuccessResponse(
            count,
            "Bookings count retrieved successfully");
    }

    // ============================================================
    // Movies Count
    // ============================================================

    public async Task<ApiResponse<int>> GetMoviesCountAsync()
    {
        _logger.LogInformation("Fetching total movies count.");

        var count = await _context.Movies.CountAsync();

        _logger.LogInformation(
            "Movies count retrieved successfully. Count: {MovieCount}.",
            count);

        return ApiResponse<int>.SuccessResponse(
            count,
            "Movies count retrieved successfully");
    }

    // ============================================================
    // Users Count
    // ============================================================

    public async Task<ApiResponse<int>> GetUsersCountAsync()
    {
        _logger.LogInformation("Fetching total users count.");

        var count = await _context.Users.CountAsync();

        _logger.LogInformation(
            "Users count retrieved successfully. Count: {UserCount}.",
            count);

        return ApiResponse<int>.SuccessResponse(
            count,
            "Users count retrieved successfully");
    }

    // ============================================================
    // Theaters Count
    // ============================================================

    public async Task<ApiResponse<TheaterCountDto>> GetTheaterCountAsync()
    {
        _logger.LogInformation("Fetching total theaters count.");

        var totalTheaters = await _context.Theaters.CountAsync();

        var dto = new TheaterCountDto
        {
            TotalTheaters = totalTheaters
        };

        _logger.LogInformation(
            "Theaters count retrieved successfully. Count: {TheaterCount}.",
            totalTheaters);

        return ApiResponse<TheaterCountDto>.SuccessResponse(
            dto,
            "Theaters count retrieved successfully");
    }
    // ============================================================
    // Screens Count
    // ============================================================

    public async Task<ApiResponse<ScreenCountDto>> GetScreenCountAsync()
    {
        _logger.LogInformation("Fetching total screens count.");

        var totalScreens = await _context.Screens.CountAsync();

        var dto = new ScreenCountDto
        {
            TotalScreens = totalScreens
        };

        _logger.LogInformation(
            "Screens count retrieved successfully. Count: {ScreenCount}.",
            totalScreens);

        return ApiResponse<ScreenCountDto>.SuccessResponse(
            dto,
            "Screens count retrieved successfully");
    }

    // ============================================================
    // Seats Count
    // ============================================================

    public async Task<ApiResponse<SeatCountDto>> GetSeatCountAsync()
    {
        _logger.LogInformation("Fetching total seats count.");

        var totalSeats = await _context.Seats.CountAsync();

        var dto = new SeatCountDto
        {
            TotalSeats = totalSeats
        };

        _logger.LogInformation(
            "Seats count retrieved successfully. Count: {SeatCount}.",
            totalSeats);

        return ApiResponse<SeatCountDto>.SuccessResponse(
            dto,
            "Seats count retrieved successfully");
    }

    // ============================================================
    // Today's Bookings
    // ============================================================

    public async Task<ApiResponse<TodayBookingsDto>> GetTodayBookingsAsync()
    {
        _logger.LogInformation("Fetching today's bookings.");

        var count = await _context.Bookings
            .CountAsync(b => b.BookingDate.Date == DateTime.UtcNow.Date);

        var dto = new TodayBookingsDto
        {
            TotalBookingsToday = count
        };

        _logger.LogInformation(
            "Today's bookings retrieved successfully. TotalBookingsToday: {BookingCount}.",
            count);

        return ApiResponse<TodayBookingsDto>.SuccessResponse(
            dto,
            "Today's bookings retrieved successfully");
    }

    // ============================================================
    // Today's Revenue
    // ============================================================

    public async Task<ApiResponse<TodayRevenueDto>> GetTodayRevenueAsync()
    {
        _logger.LogInformation("Fetching today's revenue.");

        var revenue = await _context.Payments
            .Where(p =>
                p.Status == PaymentStatus.Success &&
                p.PaymentDate.Date == DateTime.UtcNow.Date)
            .SumAsync(p => p.Amount);

        var dto = new TodayRevenueDto
        {
            RevenueToday = revenue
        };

        _logger.LogInformation(
            "Today's revenue retrieved successfully. RevenueToday: {Revenue}.",
            revenue);

        return ApiResponse<TodayRevenueDto>.SuccessResponse(
            dto,
            "Today's revenue retrieved successfully");
    }

    // ============================================================
    // Upcoming Shows
    // ============================================================

    public async Task<ApiResponse<UpcomingShowsDto>> GetUpcomingShowsAsync()
    {
        _logger.LogInformation("Fetching upcoming shows count.");

        var count = await _context.Shows
            .CountAsync(s => s.ShowDateTime > DateTime.UtcNow);

        var dto = new UpcomingShowsDto
        {
            UpcomingShows = count
        };

        _logger.LogInformation(
            "Upcoming shows count retrieved successfully. Count: {UpcomingShowCount}.",
            count);

        return ApiResponse<UpcomingShowsDto>.SuccessResponse(
            dto,
            "Upcoming shows retrieved successfully");
    }

    // ============================================================
    // Average Ticket Price
    // ============================================================

    public async Task<ApiResponse<AverageTicketPriceDto>> GetAverageTicketPriceAsync()
    {
        _logger.LogInformation("Calculating average ticket price.");

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

        _logger.LogInformation(
            "Average ticket price calculated successfully. Average: {AverageTicketPrice}.",
            average);

        return ApiResponse<AverageTicketPriceDto>.SuccessResponse(
            dto,
            "Average ticket price retrieved successfully");
    }
    // ============================================================
    // Occupancy
    // ============================================================

    public async Task<ApiResponse<OccupancyDto>> GetOccupancyAsync()
    {
        _logger.LogInformation("Calculating theater occupancy statistics.");

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

        _logger.LogInformation(
            "Occupancy statistics calculated successfully. TotalSeats: {TotalSeats}, BookedSeats: {BookedSeats}, Occupancy: {OccupancyPercentage}%.",
            totalSeats,
            bookedSeats,
            dto.OccupancyPercentage);

        return ApiResponse<OccupancyDto>.SuccessResponse(
            dto,
            "Occupancy retrieved successfully");
    }

    // ============================================================
    // Recent Bookings
    // ============================================================

    public async Task<ApiResponse<List<RecentBookingDto>>> GetRecentBookingsAsync()
    {
        _logger.LogInformation("Fetching recent bookings.");

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

        _logger.LogInformation(
            "Retrieved {BookingCount} recent bookings.",
            bookings.Count);

        return ApiResponse<List<RecentBookingDto>>.SuccessResponse(
            bookings,
            "Recent bookings retrieved successfully");
    }

    // ============================================================
    // Popular Movies
    // ============================================================

    public async Task<ApiResponse<List<PopularMovieDto>>> GetPopularMoviesAsync()
    {
        _logger.LogInformation("Fetching top 5 popular movies.");

        var movies = await _context.Bookings
            .GroupBy(b => new
            {
                b.Show.MovieId,
                b.Show.Movie.Title
            })
            .Select(g => new PopularMovieDto
            {
                MovieId = (int)g.Key.MovieId,
                Title = g.Key.Title,
                BookingCount = g.Count()
            })
            .OrderByDescending(x => x.BookingCount)
            .Take(5)
            .ToListAsync();

        _logger.LogInformation(
            "Retrieved {MovieCount} popular movies.",
            movies.Count);

        return ApiResponse<List<PopularMovieDto>>.SuccessResponse(
            movies,
            "Popular movies retrieved successfully");
    }

    // ============================================================
    // Top Customers
    // ============================================================

    public async Task<ApiResponse<List<TopCustomerDto>>> GetTopCustomersAsync()
    {
        _logger.LogInformation("Fetching top customers.");

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

        _logger.LogInformation(
            "Retrieved {CustomerCount} top customers.",
            customers.Count);

        return ApiResponse<List<TopCustomerDto>>.SuccessResponse(
            customers,
            "Top customers retrieved successfully");
    }

    // ============================================================
    // Recent Payments
    // ============================================================

    public async Task<ApiResponse<List<RecentPaymentDto>>> GetRecentPaymentsAsync()
    {
        _logger.LogInformation("Fetching recent payments.");

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

        _logger.LogInformation(
            "Retrieved {PaymentCount} recent payments.",
            payments.Count);

        return ApiResponse<List<RecentPaymentDto>>.SuccessResponse(
            payments,
            "Recent payments retrieved successfully");
    }

    // ============================================================
    // Payment Statistics
    // ============================================================

    public async Task<ApiResponse<PaymentStatisticsDto>> GetPaymentStatisticsAsync()
    {
        _logger.LogInformation("Fetching payment statistics.");

        var successfulPayments = await _context.Payments
            .CountAsync(p => p.Status == PaymentStatus.Success);

        var pendingPayments = await _context.Payments
            .CountAsync(p => p.Status == PaymentStatus.Pending);

        var failedPayments = await _context.Payments
            .CountAsync(p => p.Status == PaymentStatus.Failed);

        var dto = new PaymentStatisticsDto
        {
            SuccessfulPayments = successfulPayments,
            PendingPayments = pendingPayments,
            FailedPayments = failedPayments
        };

        _logger.LogInformation(
            "Payment statistics retrieved successfully. Success: {Successful}, Pending: {Pending}, Failed: {Failed}.",
            successfulPayments,
            pendingPayments,
            failedPayments);

        return ApiResponse<PaymentStatisticsDto>.SuccessResponse(
            dto,
            "Payment statistics retrieved successfully");
    }
}