using GameStore.API.Extensions;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.ViewModels.MinimumSpecification;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MinSpecController: ControllerBase
{
        private readonly IMinSpecificationService _minSpecService;
        private readonly IPlatformService _platformService;
        private readonly ILogger<MinSpecController> _logger;
        public MinSpecController(IMinSpecificationService minSpecService, IPlatformService platformService, 
            ILogger<MinSpecController> logger)
        {
            _minSpecService = minSpecService;
            _platformService = platformService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetMinSpecifications()
        {
            try
            {
                var response = await _minSpecService.GetMinSpecsAsync();
                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<MinSpecController>?, MinSpecController>(exception, _logger);
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
                var response = Catcher.CatchError<MinimumSpecification?, MinSpecController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }
        
        [HttpPost]
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
                var response = Catcher.CatchError<bool?, MinSpecController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPut("{id}")]
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
                var response = Catcher.CatchError<MinimumSpecification?, MinSpecController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }
        
        [HttpDelete("{id}")]
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
                var response = Catcher.CatchError<bool?, MinSpecController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }
}