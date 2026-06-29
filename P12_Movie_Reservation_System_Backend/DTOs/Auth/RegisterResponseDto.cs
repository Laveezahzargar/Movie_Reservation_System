namespace P12_Movie_Reservation_System_Backend.DTOs.Auth;

public class RegisterResponseDto
{
    public int UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}