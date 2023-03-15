using GameStore.API.Extensions;
using GameStore.Domain.Constants;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.ViewModels.Platform;
using GameStore.Domain.ViewModels.Publisher;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformService _platformService;
        private readonly ILogger<PlatformsController> _logger;
        public PlatformsController(IPlatformService platformService, ILogger<PlatformsController> logger)
        {
            _platformService = platformService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlatforms()
        {
            try
            {
                var response = await _platformService.GetPlatformsAsync();
                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<Platform>?, PlatformsController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlatformById(int id)
        {
            try
            {
                var response = await _platformService.GetPlatformByIdAsync(id);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Platform?, PlatformsController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlatform(int id)
        {
            try
            {
                var response = await _platformService.DeletePlatformAsync(id);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, PlatformsController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlatform([FromBody] PlatformViewModel publisherView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.AllErrors();
                    return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
                }

                var response = await _platformService.CreatePlatformAsync(publisherView);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return CreatedAtAction(nameof(GetPlatformById), new { id = response.Data?.Id }, response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, PlatformsController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlatform(int id, [FromBody] PlatformViewModel publisherView)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(MessageResponse.IncorrectId);
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.AllErrors();
                    return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
                }

                var response = await _platformService.UpdatePlatformAsync(id, publisherView);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Platform?, PlatformsController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }
    }
}
