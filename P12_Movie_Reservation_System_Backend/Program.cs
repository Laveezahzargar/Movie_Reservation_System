using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using P12_Movie_Reservation_System_Backend.Configurations;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.Helpers;
using P12_Movie_Reservation_System_Backend.Interfaces;
using P12_Movie_Reservation_System_Backend.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings =
    builder.Configuration
           .GetSection("Jwt")
           .Get<JwtSettings>();

if (jwtSettings == null)
    throw new Exception("Jwt configuration is missing");

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Key)
                )
        };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IMovieService, MovieService>();

builder.Services.AddScoped<IActorService, ActorService>();

builder.Services.AddScoped<ITheaterService, TheaterService>();

builder.Services.AddScoped<IScreenService, ScreenService>();

builder.Services.AddScoped<ISeatService, SeatService>();

builder.Services.AddScoped<IShowService, ShowService>();

builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddScoped<IAdminService, AdminService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();