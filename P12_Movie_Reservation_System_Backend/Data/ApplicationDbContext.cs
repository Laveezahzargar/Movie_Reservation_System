
using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Models.DomainModels;
using P12_Movie_Reservation_System_Backend.Models.JunctionModels;

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

        // ============================================================
        // Unique Constraints
        // ============================================================

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Payment>()
            .HasIndex(p => p.TransactionId)
            .IsUnique();

        modelBuilder.Entity<Ticket>()
            .HasIndex(t => t.Number)
            .IsUnique();

        modelBuilder.Entity<Seat>()
            .HasIndex(s => new { s.ScreenId, s.Number })
            .IsUnique();

        modelBuilder.Entity<ShowSeat>()
            .HasIndex(ss => new { ss.ShowId, ss.SeatId })
            .IsUnique();
    }
}