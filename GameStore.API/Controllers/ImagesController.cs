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
            var fullPath = Request.Scheme + "://" + Request.Host + Request.Path;
            //var response = await _imageService.GetImagesAsync();
            var response = new Response<List<string>>()
            {
                Data = new List<string>(){ $"{fullPath}123.jpg", $"{fullPath}12.jpg" },
                Message = "Successful",
                Status = HttpStatusCode.Ok
            };
            
            return Ok(response);
        }
        catch (Exception exception) 
        { 
            var response = Catcher.CatchError<List<Image>?, ImagesController>(exception, _logger); 
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpGet("{gameId}/{nameImage}")]
    public async Task<IActionResult> GetImageById(int gameId, string nameImage)
        {
            try
            {
                var pathImage = _environment.WebRootPath + @"\img\1\" + nameImage;
                
                // var response = await _imageService.GetImageByIdAsync(id);
                // if ((int)response.Status >= 300)
                // {
                //     return StatusCode((int)response.Status, response);
                // }
                
                return PhysicalFile(pathImage, "image/jpg");
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Image?, ImagesController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteDeveloper(int id)
        // {
        //     try
        //     {
        //         var response = await _developerService.DeleteDeveloperAsync(id);
        //         if ((int)response.Status >= 300)
        //         {
        //             return StatusCode((int)response.Status, response);
        //         }
        //
        //         return NoContent();
        //     }
        //     catch (Exception exception)
        //     {
        //         var response = Catcher.CatchError<bool?, DevelopersController>(exception, _logger);
        //         return StatusCode((int)response.Status, response);
        //     }
        // }
        //
        // [HttpPost]
        // public async Task<IActionResult> CreateDeveloper([FromBody] DeveloperViewModel developerView)
        // {
        //     try
        //     {
        //         if (!ModelState.IsValid)
        //         {
        //             var errors = ModelState.AllErrors();
        //             return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
        //         }
        //
        //         var response = await _developerService.CreateDeveloperAsync(developerView);
        //         if ((int)response.Status >= 300)
        //         {
        //             return StatusCode((int)response.Status, response);
        //         }
        //
        //         return CreatedAtAction(nameof(GetDeveloperById), new { id = response.Data?.Id }, response);
        //     }
        //     catch (Exception exception)
        //     {
        //         var response = Catcher.CatchError<bool?, DevelopersController>(exception, _logger);
        //         return StatusCode((int)response.Status, response);
        //     }
        // }
        //
        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateDeveloper(int id, [FromBody] DeveloperViewModel developerView)
        // {
        //     try
        //     {
        //         if (id <= 0)
        //         {
        //             return BadRequest(MessageResponse.IncorrectId);
        //         }
        //
        //         if (!ModelState.IsValid)
        //         {
        //             var errors = ModelState.AllErrors();
        //             return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
        //         }
        //
        //         var response = await _developerService.UpdateDeveloperAsync(id, developerView);
        //         if ((int)response.Status >= 300)
        //         {
        //             return StatusCode((int)response.Status, response);
        //         }
        //
        //         return NoContent();
        //     }
        //     catch (Exception exception)
        //     {
        //         var response = Catcher.CatchError<Developer?, DevelopersController>(exception, _logger);
        //         return StatusCode((int)response.Status, response);
        //     }
        // }
}