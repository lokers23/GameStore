using GameStore.API.Extensions;
using GameStore.Domain.Constants;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.ViewModels.Image;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly ILogger<ImagesController> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly IGameService _gameService;

    public ImagesController(IImageService imageService, ILogger<ImagesController> logger, IWebHostEnvironment environment, IGameService gameService)
    {
        _imageService = imageService;
        _logger = logger;
        _environment = environment;
        _gameService = gameService;
    }

    [HttpGet("{gameId}")]

    public async Task<IActionResult> GetImages(int gameId)
    {
        try
        {
            var response = await _imageService.GetImagesAsync();
            return Ok(response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<Image>?, ImagesController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateImage(int gameId, [FromForm] IFormFile? image)
    {
        try
        {
            if (image == null)
            {
                ModelState.AddModelError("image", "Укажите изображение");
            }
            else if (!image.ContentType.Equals("image/jpeg"))
            {
                ModelState.AddModelError("image", "Изображение должно быть в формате JPG");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.AllErrors();
                return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
            }

            var responseWithGame = await _gameService.GetGameByIdAsync(gameId);
            if (responseWithGame.Data == null)
            {
                return StatusCode((int)responseWithGame.Status, responseWithGame);
            }

            var fileName = responseWithGame.Data.Name + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".jpg";
            var imageViewModel = new ImageViewModel()
            {
                GameId = gameId,
                Name = fileName
            };

            var response = await _imageService.CreateImageAsync(imageViewModel);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            await SaveGameImage(gameId, fileName, image);

            // возможно стоит просто возвращать ОК...
            return CreatedAtAction(nameof(GetGameImage), new { gameId = gameId, fileName = fileName }, response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, ImagesController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteImage(int id)
    {
        try
        {
            var image = (await _imageService.GetImageByIdAsync(id)).Data;
            var response = await _imageService.DeleteImageAsync(id);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            await DeleteGameImage(image.Game.Id.Value, image.Name);
            return NoContent();
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, ImagesController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpGet("images/{gameId:int}/{fileName}")]
    public async Task<IActionResult> GetGameImage(int gameId, string fileName)
    {
        try
        {
            var fullPath = Path.Combine(GetPathToGameImage(gameId), fileName);
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }

            return PhysicalFile(fullPath, "image/jpg");
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<string?, ImagesController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    private async Task<bool> SaveGameImage(int id, string fileName, IFormFile image)
    {
        try
        {
            var path = GetPathToGameImage(id);
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

    private async Task<bool> DeleteGameImage(int id, string fileName)
    {
        try
        {
            var fullPath = Path.Combine(GetPathToGameImage(id), fileName);
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

    private string GetPathToGameImage(int id) =>
        Path.Combine(_environment.WebRootPath, "images", $"{id}");
}