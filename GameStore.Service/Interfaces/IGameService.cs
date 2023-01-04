using GameStore.Domain.Models;
using GameStore.Domain.Response;

namespace GameStore.Service.Interfaces
{
    public interface IGameService
    {
        Task<Response<List<Game>>> GetGamesAsync();
        Task<Response<Game>> GetGameById(int id);
        Task<Response<bool>> DeleteGameById(int id);
        Task<Response<bool>> CreateGame(Game game);
        Task<Response<bool>> UpdateGame(Game game);
    }
}
