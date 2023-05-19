using GameStore.API.Extensions;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Game;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.ViewModels.Game;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly IDeveloperService _developerService;
    private readonly IPublisherService _publisherService;
    private readonly IMinSpecificationService _minSpecificationService;
    private readonly IGenreService _genreService;
    private readonly ILogger<GamesController> _logger;
    private readonly IWebHostEnvironment _environment;

    public GamesController(IGameService gameService, IDeveloperService developerService, IPublisherService publisherService, 
        IMinSpecificationService minSpecificationService, IGenreService genreService, ILogger<GamesController> logger, IWebHostEnvironment environment)
    {
        _gameService = gameService;
        _developerService = developerService;
        _publisherService = publisherService;
        _minSpecificationService = minSpecificationService;
        _genreService = genreService;
        _logger = logger;
        _environment = environment;
    }

    [HttpGet]
    
    public async Task<IActionResult> GetGames()
    {
        try
        {
            var response = await _gameService.GetGamesAsync();
            return Ok(response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<GameDto>?, GamesController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGameById(int id)
    {
        try
        {
            var response = await _gameService.GetGameByIdAsync(id);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            return Ok(response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<GameDto?, GamesController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateGame([FromForm] GameViewModel gameViewModel, IFormFile? avatar)
    {
        try
        {
            if (avatar == null)
            {
                ModelState.AddModelError("Avatar", "Укажите изображение");
            }
            else if (!avatar.ContentType.Equals("image/jpeg"))
            {
                ModelState.AddModelError("Avatar", "Изображение должно быть в формате JPG");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.AllErrors();
                return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
            }

            var errorsExistence = await CheckExistsEntities(gameViewModel);
            if (errorsExistence != null)
            {     
                return BadRequest(new { Message = MessageResponse.Invalid, Errors = errorsExistence });
            }

            var fileName = gameViewModel.Name + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".jpg";
            gameViewModel.AvatarName = fileName;

            var response = await _gameService.CreateGameAsync(gameViewModel);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            var isCreatedImage = await SaveAvatarImage(fileName, avatar!);
            return CreatedAtAction(nameof(GetGameById), new { id = response.Data?.Id }, response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, GamesController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateGame(int id, [FromForm] GameViewModel gameViewModel, IFormFile? avatar)
    {
        try
        {
            if (gameViewModel.isChangedAvatar && avatar == null)
            {
                ModelState.AddModelError("Avatar", "Укажите изображение");
            }
            else if (gameViewModel.isChangedAvatar && !avatar.ContentType.Equals("image/jpeg"))
            {
                ModelState.AddModelError("Avatar", "Изображение должно быть в формате JPG");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.AllErrors();
                return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
            }

            var errorsExistence = await CheckExistsEntities(gameViewModel);
            if (errorsExistence != null)
            {
                return BadRequest(new { Message = MessageResponse.Invalid, Errors = errorsExistence });
            }

            var responseWithGame = await _gameService.GetGameByIdAsync(id);
            var fileNameForDelete = responseWithGame.Data.AvatarName;

            // думаю эту логику нужно вынести в сервис и проверять там меняем ли мы аватарку
            //var fileName = gameViewModel.Name + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".jpg";
            //gameViewModel.AvatarName = fileName;

            var response = await _gameService.UpdateGameAsync(id, gameViewModel);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            if (gameViewModel.isChangedAvatar)
            {
                await DeleteAvatarImage(fileNameForDelete);
                //await SaveAvatarImage(fileName, avatar);
                await SaveAvatarImage(response.Data.AvatarName, avatar);
            }
            

            return NoContent();
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<GameDto, GamesController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        try
        {
            var responseWithGame = await _gameService.GetGameByIdAsync(id);
            var fileName = responseWithGame.Data!.AvatarName;

            var response = await _gameService.DeleteGameAsync(id);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            await DeleteAvatarImage(fileName!);
            return NoContent();
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, GamesController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpGet("avatars/{fileName}")]
    public async Task<IActionResult> GetAvatarImage(string fileName)
    {
        try
        {
            var fullPath = Path.Combine(GetPathToAvatar(), fileName);
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }

            return PhysicalFile(fullPath, "image/jpg");
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<string?, GamesController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    private async Task<bool> SaveAvatarImage(string fileName, IFormFile image)
    {
        try
        {
            var path = GetPathToAvatar();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fullPath = Path.Combine(path, fileName);
            await using var stream = new FileStream(fullPath, FileMode.Create);
            await image.CopyToAsync(stream);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> DeleteAvatarImage(string fileName)
    {
        try
        {
            var fullPath = Path.Combine(GetPathToAvatar(), fileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    private string GetPathToAvatar() =>
         Path.Combine(_environment.WebRootPath, "images", "avatars");


    private async Task<Dictionary<string, string[]>?> CheckExistsEntities(GameViewModel gameViewModel)
    {
        var developer = await _developerService.GetDeveloperByIdAsync(gameViewModel.DeveloperId.Value);
        if (developer.Data == null)
        {
            return new Dictionary<string, string[]>() { { "DeveloperId", new string[] { developer.Message } } };
        }

        var publisher = await _publisherService.GetPublisherByIdAsync(gameViewModel.PublisherId.Value);
        if (publisher.Data == null)
        {
            return new Dictionary<string, string[]>() { { "PublisherId", new string[] { publisher.Message } } };
        }

        foreach (var id in gameViewModel.GenreIds)
        {
            var genre = await _genreService.GetGenreByIdAsync(id);
            if (genre.Data == null)
            {
                return new Dictionary<string, string[]>() { { "GenreIds", new string[] { genre.Message } } };
            }
        }

        foreach (var id in gameViewModel.MinimumSpecificationIds)
        {
            var minSpec = await _minSpecificationService.GetMinSpecByIdAsync(id);
            if (minSpec.Data == null)
            {
                return new Dictionary<string, string[]>() { { "MinimumSpecificationIds", new string[] { minSpec.Message } } };
            }
        }

        return null;
    }
}