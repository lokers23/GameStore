using GameStore.API.Extensions;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.MinimumSpecification;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.ViewModels.MinimumSpecification;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MinSpecsController : ControllerBase
{
    private readonly IMinSpecificationService _minSpecService;
    private readonly IPlatformService _platformService;
    private readonly ILogger<MinSpecsController> _logger;
    public MinSpecsController(IMinSpecificationService minSpecService, IPlatformService platformService,
        ILogger<MinSpecsController> logger)
    {
        _minSpecService = minSpecService;
        _platformService = platformService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetMinSpecifications(string? platformName, [FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string? operatingSystem, 
        [FromQuery] string? processor, [FromQuery] string? graphics)
    {
        try
        {
            var response = await _minSpecService.GetMinSpecsAsync(platformName, page, pageSize, operatingSystem, processor, graphics);
            return Ok(response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<MinSpecDto>?, MinSpecsController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMinSpecificationsById(int id)
    {
        try
        {
            var response = await _minSpecService.GetMinSpecByIdAsync(id);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            return Ok(response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<MinSpecDto?, MinSpecsController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpPost]
    [Authorize(nameof(AccessRole.Moderator))]
    public async Task<IActionResult> CreateMinSpecification([FromBody] MinSpecificationViewModel minSpecView)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var isExist = await _platformService.GetPlatformByIdAsync(minSpecView.PlatformId ?? 0);
                if (isExist.Status == HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError("PlatformId", "Такой платформы не существует");
                }

                var errors = ModelState.AllErrors();
                return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
            }

            var response = await _minSpecService.CreateMinSpecAsync(minSpecView);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            return CreatedAtAction(nameof(GetMinSpecificationsById), new { id = response.Data?.Id }, response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, MinSpecsController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpPut("{id}")]
    [Authorize(nameof(AccessRole.Moderator))]
    public async Task<IActionResult> UpdateMinSpecification(int id, [FromBody] MinSpecificationViewModel minSpecView)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(MessageResponse.IncorrectId);
            }

            if (!ModelState.IsValid)
            {
                var isExist = await _platformService.GetPlatformByIdAsync(minSpecView.PlatformId ?? 0);
                if (isExist.Status == HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError("PlatformId", "Такой платформы не существует");
                }

                var errors = ModelState.AllErrors();
                return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
            }

            var response = await _minSpecService.UpdateMinSpecAsync(id, minSpecView);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            return NoContent();
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<MinSpecDto?, MinSpecsController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(nameof(AccessRole.Moderator))]
    public async Task<IActionResult> DeleteMinSpecification(int id)
    {
        try
        {
            var response = await _minSpecService.DeleteMinSpecAsync(id);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            return NoContent();
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, MinSpecsController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }
}