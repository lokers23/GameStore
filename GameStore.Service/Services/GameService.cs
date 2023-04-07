﻿using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Game;
using GameStore.Domain.Dto.Genre;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Game;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services;

public class GameService: IGameService
{
    private readonly IRepository<Game> _gameRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GameService> _logger;
    public GameService(ILogger<GameService> logger, IRepository<Game> gameRepository, IMapper mapper)
    {
        _logger = logger;
        _gameRepository = gameRepository;
        _mapper = mapper;
    }
    public async Task<Response<List<GameDto>?>> GetGamesAsync()
    {
        try
        {
            var response = new Response<List<GameDto>?>();
            var games = await _gameRepository.GetAll()
                .Include(game => game.Publisher)
                .Include(game => game.Developer)
                .Select(game => _mapper.Map<GameDto>(game))
                .ToListAsync();
            
            response.Data = games;
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<GameDto>?, GameService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<GameDto?>> GetGameByIdAsync(int id)
    {
        try
        {
            var response = new Response<GameDto?>();
            var game = await _gameRepository.GetAll()
                .Include(game => game.Publisher)
                .Include(game => game.Developer)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (game == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            response.Data = _mapper.Map<GameDto>(game);
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<GameDto?, GameService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<GameDto?>> CreateGameAsync(GameViewModel gameViewModel)
    {
        try
        {
            var response = new Response<GameDto?>();
            var responseExist = await CheckExistAsync(gameViewModel);
            if (responseExist.Data)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;

                return response;
            }

            var game = _mapper.Map<Game>(gameViewModel);
            await _gameRepository.CreateAsync(game);
            
            response.Status = HttpStatusCode.Created;
            response.Data = _mapper.Map<GameDto>(game);
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<GameDto?, GameService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<GameDto?>> UpdateGameAsync(int id, GameViewModel gameViewModel)
    {
        try
        {
            var response = new Response<GameDto?>();
            var game = await _gameRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (game == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            var responseExist = await CheckExistAsync(gameViewModel, id);
            if (responseExist.Data)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;
                return response;
            }

            game.Name = gameViewModel.Name;
            game.DeveloperId = gameViewModel.DeveloperId;
            game.PublisherId = gameViewModel.PublisherId;
            game.Description = gameViewModel.Description;
            game.ReleaseOn = gameViewModel.ReleaseOn ?? DateTime.Now;
            game.Price = gameViewModel.Price ?? 0.0m;
            game.VideoUrl = gameViewModel.VideoUrl;
            game.AvatarName = gameViewModel.AvatarName;

            await _gameRepository.UpdateAsync(game);
            
            response.Data = _mapper.Map<GameDto>(game);
            response.Status = HttpStatusCode.NoContent;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<GameDto?, GameService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<bool?>> DeleteGameAsync(int id)
    {
        try
        {
            var response = new Response<bool?>();
            var game = await _gameRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (game == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            await _gameRepository.DeleteAsync(game);

            response.Status = HttpStatusCode.NoContent;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, GameService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<bool>> CheckExistAsync(GameViewModel gameViewModel, int id = 0)
    {
        var response = new Response<bool>()
        {
            Data = false,
            Errors = new Dictionary<string, string[]>()
        };

        var isExist = await _gameRepository.GetAll().AnyAsync(game =>
                game.Id != id &&
                game.Name.Equals(gameViewModel.Name) &&
                game.DeveloperId == gameViewModel.DeveloperId &&
                game.PublisherId == gameViewModel.PublisherId &&
                game.ReleaseOn == gameViewModel.ReleaseOn);

        if (isExist)
        {
            response.Status = HttpStatusCode.Conflict;
            response.Data = true;
            response.Errors.Add(nameof(Game), new[] { MessageError.EntityExists });
        }

        return response;
    }
}