using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Game;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Game;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services;

public class GameService : IGameService
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
    public async Task<Response<List<GameDto>?>> GetGamesAsync(string? sort, string? genre, string? name, decimal? minPrice, decimal? maxPrice,
        int? activationId, int? platformId)
    {
        try
        {
            var response = new Response<List<GameDto>?>();
            IQueryable<Game> games = _gameRepository.GetAll()
                .Include(game => game.Publisher)
                .Include(game => game.Developer)
                .Include((game => game.Activation))
                .Include(game => game.GameMinSpecifications)
                .ThenInclude(gm => gm.MinimumSpecification)
                .ThenInclude(ms => ms.Platform)
                .Include(game => game.GameGenres)
                .ThenInclude(gg => gg.Genre);

            games = games.Where(game => 
                (string.IsNullOrEmpty(genre) || game.GameGenres.Any(g => genre == g.Genre.Name)) && 
                (string.IsNullOrEmpty(name) || game.Name.StartsWith(name)) &&
                (!minPrice.HasValue || game.Price >= minPrice.Value) &&
                (!maxPrice.HasValue || game.Price <= maxPrice.Value) &&
                (!maxPrice.HasValue || game.Price <= maxPrice.Value) && 
                (!activationId.HasValue || game.ActivationId == activationId) &&
                (!platformId.HasValue || game.GameMinSpecifications.Any(ms => platformId == ms.MinimumSpecification.PlatformId))
                );
            
            switch (sort)
            {
                case "date":
                    games = games.OrderBy(game => game.ReleaseOn);
                    break;
                case "price":
                    games = games.OrderBy(game => game.Price);
                    break;
                case "name":
                    games = games.OrderBy(game => game.Name);
                    break;
                case "id_desc":
                    games = games.OrderByDescending(game => game.Id);
                    break;
                case "date_desc":
                    games = games.OrderByDescending(game => game.ReleaseOn);
                    break;
                case "price_desc":
                    games = games.OrderByDescending(game => game.Price);
                    break;
                case "name_desc":
                    games = games.OrderByDescending(game => game.Name);
                    break;
            }
            
            response.Data = await games.Select(game => _mapper.Map<GameDto>(game))
                .ToListAsync();
            
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
                .Include((game => game.Activation))
                .Include(game => game.GameMinSpecifications)
                    .ThenInclude(gm => gm.MinimumSpecification)
                        .ThenInclude(ms => ms.Platform)
                .Include(game => game.GameGenres)
                    .ThenInclude(gg => gg.Genre)
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

            var gameGenres = new List<GameGenre>();
            foreach (var genreId in gameViewModel.GenreIds)
            {
                var gameGenre = new GameGenre { Game = game, GenreId = genreId };
                gameGenres.Add(gameGenre);
            }

            var gameMinSpecs = new List<GameMinSpecification>();
            foreach (var minSpecId in gameViewModel.MinimumSpecificationIds)
            {
                var gameMinSpec = new GameMinSpecification { Game = game, MinimumSpecificationId = minSpecId };
                gameMinSpecs.Add(gameMinSpec);
            }

            game.GameGenres = gameGenres;
            game.GameMinSpecifications = gameMinSpecs;
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
                .Include(game => game.Publisher)
                .Include(game => game.Developer)
                .Include((game => game.Activation))
                .Include(game => game.GameMinSpecifications)
                .Include(game => game.GameGenres)
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
            game.ActivationId = gameViewModel.ActivationId;
            game.Description = gameViewModel.Description;
            game.ReleaseOn = gameViewModel.ReleaseOn ?? DateTime.Now;
            game.Price = gameViewModel.Price ?? 0.0m;
            game.VideoUrl = gameViewModel.VideoUrl;
            //game.AvatarName = gameViewModel.AvatarName;
            if (gameViewModel.isChangedAvatar)
            {
                game.AvatarName =  $"{gameViewModel.Name}-{DateTime.Now:yyyy-MM-dd}.jpg";
            }
            
            await _gameRepository.UpdateAsync(game);

            var gameGenres = new List<GameGenre>();
            foreach (var genreId in gameViewModel.GenreIds)
            {
                var gameGenre = new GameGenre { GameId = id, GenreId = genreId };
                gameGenres.Add(gameGenre);
            }

            var gameMinSpecs = new List<GameMinSpecification>();
            foreach (var minSpecId in gameViewModel.MinimumSpecificationIds)
            {
                var gameMinSpec = new GameMinSpecification { GameId = id, MinimumSpecificationId = minSpecId };
                gameMinSpecs.Add(gameMinSpec);
            }

            game.GameGenres = gameGenres;
            game.GameMinSpecifications = gameMinSpecs;

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
                game.PublisherId == gameViewModel.PublisherId);
                //game.ReleaseOn == gameViewModel.ReleaseOn);

        if (isExist)
        {
            response.Status = HttpStatusCode.Conflict;
            response.Data = true;
            response.Errors.Add(nameof(Game), new[] { MessageError.EntityExists });
        }

        return response;
    }
}