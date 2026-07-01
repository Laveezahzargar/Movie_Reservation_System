using Microsoft.EntityFrameworkCore;
using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.Data.ApplicationDbContext;
using P12_Movie_Reservation_System_Backend.DTOs.Actor;
using P12_Movie_Reservation_System_Backend.Interfaces;
using P12_Movie_Reservation_System_Backend.Models.DomainModels;

namespace P12_Movie_Reservation_System_Backend.Services;

public class ActorService : IActorService
{
    private readonly ApplicationDbContext _context;

    public ActorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<ActorListDto>>> GetAllActorsAsync()
    {
        var actors = await _context.Actors.ToListAsync();

        var result = actors.Select(a => new ActorListDto
        {
            ActorId = a.ActorId,
            Name = a.ActorName
        }).ToList();

        return ApiResponse<List<ActorListDto>>
            .SuccessResponse(result, "Actors Retrieved Successfully");
    }

    public async Task<ApiResponse<ActorDetailDto>> CreateActorAsync(CreateActorDto request)
    {
        var actor = new Actor
        {
            ActorName = request.Name
        };

        await _context.Actors.AddAsync(actor);
        await _context.SaveChangesAsync();

        return ApiResponse<ActorDetailDto>.SuccessResponse(
            new ActorDetailDto
            {
                ActorId = actor.ActorId,
                Name = actor.ActorName
            },
            "Actor Created Successfully");
    }

    public async Task<ApiResponse<List<ActorMovieDto>>> GetActorMoviesAsync(int actorId)
    {
        var actorExists = await _context.Actors.AnyAsync(a => a.ActorId == actorId);

        if (!actorExists)
            return ApiResponse<List<ActorMovieDto>>
                .FailureResponse("Actor not found");

        var movies = await _context.MovieActors
            .Where(ma => ma.ActorId == actorId)
            .Select(ma => new ActorMovieDto
            {
                MovieId = ma.Movie.MovieId,
                Title = ma.Movie.Title,
                Genre = ma.Movie.Genre,
                ReleaseDate = ma.Movie.ReleaseDate
            })
            .ToListAsync();

        return ApiResponse<List<ActorMovieDto>>
            .SuccessResponse(movies, "Actor Movies Retrieved Successfully");
    }
}