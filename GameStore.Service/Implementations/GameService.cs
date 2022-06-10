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
using GameStore.Domain.ViewModels.Game;

namespace GameStore.Service.Implementations
{
    public class GameService : IGameService
    {
        private readonly IGameAction _gameAction;

        public GameService(IGameAction gameAction)
        {
            _gameAction = gameAction;
        }

        public async Task<IBaseResponse<GameViewModel>> CreateGame(GameViewModel gameViewModel)
        {
            var baseResponse = new BaseResponse<GameViewModel>();
            try
            {
                var game = new Game()
                {
                    Name = gameViewModel.Name,
                    Created = gameViewModel.Created,
                    Discription = gameViewModel.Discription,
                    Price = gameViewModel.Price
                };

                await _gameAction.Create(game);

                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<GameViewModel>()
                {
                    Discription = $"[GetGameByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteGame(int id)
        {
            try
            {
                var game = await _gameAction.Get(id);
                if (game == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Discription = "Game not found",
                        StatusCode = StatusCode.GameNotFound,
                        Data = false
                    };
                }

                await _gameAction.Delete(game);
                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };
                
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Discription = $"[GetGameByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Game>> GetGameByName(string name)
        {
            var baseResponse = new BaseResponse<Game>();
            try
            {
                var game = await _gameAction.GetByName(name);
                if (game == null)
                {
                    baseResponse.Discription = "Game not found";
                    baseResponse.StatusCode = StatusCode.GameNotFound;
                    return baseResponse;
                }

                baseResponse.Data = game;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Game>()
                {
                    Discription = $"[GetGameByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Game>> GetGame(int id)
        {
            var baseResponse = new BaseResponse<Game>();
            try
            {
                var game = await _gameAction.Get(id);
                if (game == null)
                {
                    baseResponse.Discription = "Game not found";
                    baseResponse.StatusCode = StatusCode.GameNotFound;
                    return baseResponse;
                }

                baseResponse.Data = game;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Game>()
                {
                    Discription = $"[GetGame] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
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
