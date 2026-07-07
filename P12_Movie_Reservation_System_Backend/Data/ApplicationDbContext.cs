
using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Models.DomainModels;
using P12_Movie_Reservation_System_Backend.Models.JunctionModels;
using P12_Movie_Reservation_System_Backend.Enums;

namespace P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<MovieActor> MovieActors { get; set; }


    public DbSet<City> Cities { get; set; }
    public DbSet<Theater> Theaters { get; set; }
    public DbSet<Screen> Screens { get; set; }
    public DbSet<Seat> Seats { get; set; }

    public DbSet<Show> Shows { get; set; }
    public DbSet<ShowSeat> ShowSeats { get; set; }

    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingSeat> BookingSeats { get; set; }

    public DbSet<Payment> Payments { get; set; }
    public DbSet<Ticket> Tickets { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ============================================================
        // MovieActor (Many-to-Many)
        // ============================================================
        modelBuilder.Entity<MovieActor>()
            .HasKey(ma => new { ma.MovieId, ma.ActorId });

        modelBuilder.Entity<MovieActor>()
            .HasOne(ma => ma.Movie)
            .WithMany(m => m.MovieActors)
            .HasForeignKey(ma => ma.MovieId);

        modelBuilder.Entity<MovieActor>()
            .HasOne(ma => ma.Actor)
            .WithMany(a => a.MovieActors)
            .HasForeignKey(ma => ma.ActorId);

        // ============================================================
        // User -> Bookings
        // ============================================================
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================
        // Theater -> Screens
        // ============================================================
        modelBuilder.Entity<Screen>()
            .HasOne(s => s.Theater)
            .WithMany(t => t.Screens)
            .HasForeignKey(s => s.TheaterId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================
        // Screen -> Seats
        // ============================================================
        modelBuilder.Entity<Seat>()
            .HasOne(s => s.Screen)
            .WithMany(sc => sc.Seats)
            .HasForeignKey(s => s.ScreenId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================
        // Movie -> Shows
        // ============================================================
        modelBuilder.Entity<Show>()
            .HasOne(s => s.Movie)
            .WithMany(m => m.Shows)
            .HasForeignKey(s => s.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================
        // Screen -> Shows
        // ============================================================
        modelBuilder.Entity<Show>()
            .HasOne(s => s.Screen)
            .WithMany(sc => sc.Shows)
            .HasForeignKey(s => s.ScreenId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================
        // Show -> ShowSeats
        // ============================================================
        modelBuilder.Entity<ShowSeat>()
            .HasOne(ss => ss.Show)
            .WithMany(s => s.ShowSeats)
            .HasForeignKey(ss => ss.ShowId)
            .OnDelete(DeleteBehavior.Cascade);

        // ============================================================
        // Seat -> ShowSeats
        // ============================================================
        modelBuilder.Entity<ShowSeat>()
            .HasOne(ss => ss.Seat)
            .WithMany(s => s.ShowSeats)
            .HasForeignKey(ss => ss.SeatId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================
        // Show -> Bookings
        // ============================================================
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Show)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.ShowId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================
        // Booking -> BookingSeats
        // ============================================================
        modelBuilder.Entity<BookingSeat>()
            .HasOne(bs => bs.Booking)
            .WithMany(b => b.BookingSeats)
            .HasForeignKey(bs => bs.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        // ============================================================
        // ShowSeat -> BookingSeats
        // ============================================================
        modelBuilder.Entity<BookingSeat>()
            .HasOne(bs => bs.ShowSeat)
            .WithMany(ss => ss.BookingSeats)
            .HasForeignKey(bs => bs.ShowSeatId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================
        // Booking <-> Payment (1:1)
        // ============================================================
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Booking)
            .WithOne(b => b.Payment)
            .HasForeignKey<Payment>(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        // ============================================================
        // Booking <-> Ticket (1:1)
        // ============================================================
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Booking)
            .WithOne(b => b.Ticket)
            .HasForeignKey<Ticket>(t => t.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        //city

        modelBuilder.Entity<Theater>()
    .HasOne(t => t.City)
    .WithMany(c => c.Theaters)
    .HasForeignKey(t => t.CityId)
    .OnDelete(DeleteBehavior.Restrict);

        // ============================================================
        // Unique Constraints
        // ============================================================

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Payment>()
            .HasIndex(p => p.TransactionId)
            .IsUnique();

        modelBuilder.Entity<Payment>()
            .HasIndex(p => p.BookingId)
            .IsUnique();

        modelBuilder.Entity<Ticket>()
            .HasIndex(t => t.TicketNumber)
            .IsUnique();

        modelBuilder.Entity<Ticket>()
            .HasIndex(t => t.BookingId)
            .IsUnique();

        modelBuilder.Entity<Seat>()
            .HasIndex(s => new { s.ScreenId, s.Number })
            .IsUnique();

        modelBuilder.Entity<ShowSeat>()
            .HasIndex(ss => new { ss.ShowId, ss.SeatId })
            .IsUnique();

        modelBuilder.Entity<Show>()
            .HasIndex(s => new
            {
                s.ScreenId,
                s.ShowDateTime
            })
            .IsUnique();

        modelBuilder.Entity<BookingSeat>()
            .HasIndex(bs => new { bs.BookingId, bs.ShowSeatId })
            .IsUnique();

        modelBuilder.Entity<Screen>()
            .HasIndex(s => new { s.TheaterId, s.ScreenName })
            .IsUnique();

        modelBuilder.Entity<Theater>()
            .HasIndex(t => new
            {
                t.TheaterName,
                t.Address
            })
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Booking>()
            .Property(b => b.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Payment>()
            .Property(p => p.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Payment>()
            .Property(p => p.PaymentMethod)
            .HasConversion<string>();

        modelBuilder.Entity<Seat>()
            .Property(s => s.Type)
            .HasConversion<string>();

        modelBuilder.Entity<ShowSeat>()
            .Property(ss => ss.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Booking>()
            .Property(b => b.TotalAmount)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(10, 2);

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasDefaultValue(Role.Customer);

        modelBuilder.Entity<Booking>()
            .Property(b => b.BookingDate)
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Payment>()
            .Property(p => p.PaymentDate)
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Ticket>()
            .Property(t => t.GeneratedAt)
            .HasDefaultValueSql("GETUTCDATE()");

    }
}