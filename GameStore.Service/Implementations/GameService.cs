using GameStore.DAL.Intefaces;
using GameStore.Domain.Entity;
using GameStore.Domain.Response;
using GameStore.Service.Interfaces;
using GameStore.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Service.Implementations
{
    public class GameService : IGameService
    {
        private readonly IGameAction _gameAction;

        public GameService(IGameAction gameAction)
        {
            _gameAction = gameAction;
        }

        public async Task<IBaseResponse<IEnumerable<Game>>> GetGames()
        {
            var baseResponse = new BaseResponse<IEnumerable<Game>>();
            try
            {
                var games = await _gameAction.Select();
                if (games.Count == 0)
                {
                    baseResponse.Discription = "Элементы не найдены";
                    baseResponse.StatusCode = StatusCode.OK;
                    return baseResponse;
                }

                baseResponse.Data = games;
                baseResponse.StatusCode = StatusCode.OK;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Game>>()
                {
                    Discription = $"[GetGames]: {ex.Message}"
                };
            }
        }
    }
}
