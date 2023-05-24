using GameStore.Domain.Dto.Game;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Game;

namespace GameStore.Service.Interfaces;

public interface IGameService
{
    Task<Response<List<GameDto>?>> GetGamesAsync(int? page, int? pageSize,string? sort, string? genre, string? name, decimal? minPrice, decimal? maxPrice,
        int? activationId, int? platformId);
    Task<Response<GameDto?>> GetGameByIdAsync(int id);
    Task<Response<GameDto?>> CreateGameAsync(GameViewModel gameViewModel);
    Task<Response<GameDto?>> UpdateGameAsync(int id, GameViewModel gameViewModel);
    Task<Response<bool?>> DeleteGameAsync(int id);
    Task<Response<bool>> CheckExistAsync(GameViewModel gameViewModel, int id);
}