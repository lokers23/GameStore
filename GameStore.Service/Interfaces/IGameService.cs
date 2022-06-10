using GameStore.Domain.Entity;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Service.Interfaces
{
    public interface IGameService
    {
        Task<IBaseResponse<IEnumerable<Game>>> GetGames();
        Task<IBaseResponse<Game>> GetGame(int id);
        Task<IBaseResponse<bool>> DeleteGame(int id);
        Task<IBaseResponse<GameViewModel>> CreateGame(GameViewModel gameViewModel);
    }
}
