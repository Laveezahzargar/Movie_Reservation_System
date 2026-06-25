namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class User
{
    public int UserId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string Role { get; set; }

    public ICollection<Booking> Bookings { get; set; }
}   