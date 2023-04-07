using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace GameStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController: ControllerBase
{
    private readonly IImageService _imageService;
    private readonly ILogger<ImagesController> _logger;
    private readonly IWebHostEnvironment _environment;
    public ImagesController(IImageService imageService, ILogger<ImagesController> logger, IWebHostEnvironment environment)
    {
        _imageService = imageService;
        _logger = logger;
        _environment = environment;
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

    [HttpGet("images/{gameId:int}/{fileName}")]
    public async Task<IActionResult> GetAvatarImage(int gameId, string fileName)
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