using GameStore.API.Extensions;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Game;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.ViewModels.Game;
using GameStore.Domain.ViewModels.Genre;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController: ControllerBase
{
    private readonly IGameService _gameService;
    private readonly ILogger<GamesController> _logger;
    private readonly IWebHostEnvironment _environment;

    public GamesController(IGameService gameService, ILogger<GamesController> logger, IWebHostEnvironment environment)
    {
        _gameService = gameService;
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
    public async Task<IActionResult> CreateGame([FromForm]GameViewModel gameViewModel, IFormFile? avatar)
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

            var responseWithGame = await _gameService.GetGameByIdAsync(id);
            var fileNameForDelete = responseWithGame.Data.AvatarName;

            var fileName = gameViewModel.Name + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".jpg";
            gameViewModel.AvatarName = fileName;
            var response = await _gameService.UpdateGameAsync(id, gameViewModel);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            await DeleteAvatarImage(fileNameForDelete!);
            await SaveAvatarImage(fileName, avatar);

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
}