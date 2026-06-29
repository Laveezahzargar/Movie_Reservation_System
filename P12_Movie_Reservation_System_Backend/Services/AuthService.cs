using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.DTOs.Auth;
using P12_Movie_Reservation_System_Backend.Enums;
using P12_Movie_Reservation_System_Backend.Helpers;
using P12_Movie_Reservation_System_Backend.Interfaces;
using P12_Movie_Reservation_System_Backend.Models.DomainModels;

namespace P12_Movie_Reservation_System_Backend.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        ApplicationDbContext context,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<AuthService> logger)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request)
    {
        try
        {
            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == request.Email);

            if (emailExists)
            {
                _logger.LogWarning("Registration failed. Email {Email} already exists.", request.Email);

                return ApiResponse<RegisterResponseDto>.FailureResponse(
                    "Email already exists.");
            }

            var user = new User
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = Role.Customer
            };

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            var response = new RegisterResponseDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString()
            };

            _logger.LogInformation(
                "User {Email} registered successfully.",
                user.Email);

            return ApiResponse<RegisterResponseDto>
                .SuccessResponse(
                    response,
                    "Registration successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Registration failed.");

            return ApiResponse<RegisterResponseDto>
                .FailureResponse(
                    "Registration failed.");
        }
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        try
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == request.Email.Trim().ToLower());

            if (user == null)
            {
                _logger.LogWarning("Login failed. Email {Email} not found.", request.Email);

                return ApiResponse<LoginResponseDto>
                    .FailureResponse("Invalid email or password.");
            }

            var isPasswordValid =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.PasswordHash);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Login failed. Invalid password for {Email}.", request.Email);

                return ApiResponse<LoginResponseDto>
                    .FailureResponse("Invalid email or password.");
            }

            var jwt =
                _jwtTokenGenerator.GenerateToken(user);

            var response = new LoginResponseDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = jwt.Token,
                ExpiresAt = jwt.ExpiresAt
            };

            _logger.LogInformation(
                "User {Email} logged in successfully.",
                user.Email);

            return ApiResponse<LoginResponseDto>
                .SuccessResponse(
                    response,
                    "Login successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Login failed.");

            return ApiResponse<LoginResponseDto>
                .FailureResponse(
                    "Login failed.");
        }
    }
}