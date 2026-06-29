using System.ComponentModel.DataAnnotations;
using P12_Movie_Reservation_System_Backend.Enums;

namespace P12_Movie_Reservation_System_Backend.Models.DomainModels;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public Role Role { get; set; } = Role.Customer;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}