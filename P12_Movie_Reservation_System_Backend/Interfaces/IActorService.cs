using P12_Movie_Reservation_System_Backend.Common;
using P12_Movie_Reservation_System_Backend.DTOs.Actor;

namespace P12_Movie_Reservation_System_Backend.Interfaces;

public interface IActorService
{
    Task<ApiResponse<List<ActorListDto>>> GetAllActorsAsync();

    Task<ApiResponse<ActorDetailDto>> CreateActorAsync(CreateActorDto request);

    Task<ApiResponse<List<ActorMovieDto>>> GetActorMoviesAsync(int actorId);
}