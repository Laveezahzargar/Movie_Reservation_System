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
    private readonly ILogger<ActorService> _logger;

    public ActorService(ApplicationDbContext context, ILogger<ActorService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResponse<List<ActorListDto>>> GetAllActorsAsync()
    {
        _logger.LogInformation("Fetching all actors.");

        var actors = await _context.Actors.ToListAsync();

        var result = actors.Select(a => new ActorListDto
        {
            ActorId = a.ActorId,
            Name = a.ActorName
        }).ToList();

        _logger.LogInformation(
            "Retrieved {ActorCount} actors successfully.",
            result.Count);

        return ApiResponse<List<ActorListDto>>
            .SuccessResponse(result, "Actors Retrieved Successfully");
    }

    public async Task<ApiResponse<ActorDetailDto>> CreateActorAsync(CreateActorDto request)
    {
        _logger.LogInformation(
            "Creating actor '{ActorName}'.",
            request.Name);

        var actor = new Actor
        {
            ActorName = request.Name
        };

        await _context.Actors.AddAsync(actor);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Actor created successfully. ActorId {ActorId}, ActorName {ActorName}.",
            actor.ActorId,
            actor.ActorName);

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
        _logger.LogInformation(
            "Fetching movies for ActorId {ActorId}.",
            actorId);

        var actorExists = await _context.Actors
            .AnyAsync(a => a.ActorId == actorId);

        if (!actorExists)
        {
            _logger.LogWarning(
                "Actor with ActorId {ActorId} was not found.",
                actorId);

            return ApiResponse<List<ActorMovieDto>>
                .FailureResponse("Actor not found");
        }

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

        _logger.LogInformation(
            "Retrieved {MovieCount} movies for ActorId {ActorId}.",
            movies.Count,
            actorId);

        return ApiResponse<List<ActorMovieDto>>
            .SuccessResponse(movies, "Actor Movies Retrieved Successfully");
    }
}