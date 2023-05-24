using GameStore.API.Extensions;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Developer;
using GameStore.Domain.Helpers;
using GameStore.Domain.ViewModels.Developer;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevelopersController : ControllerBase
    {
        private readonly IDeveloperService _developerService;
        private readonly ILogger<DevelopersController> _logger;
        public DevelopersController(IDeveloperService developerService, ILogger<DevelopersController> logger)
        {
            _developerService = developerService;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles = nameof(AccessRole.Administrator))]
        public async Task<IActionResult> GetDevelopers([FromQuery] int? page, [FromQuery] int? pageSize)
        {
            try
            {
                var response = await _developerService.GetDevelopersAsync(page, pageSize);
                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<DeveloperDto>?, DevelopersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeveloperById(int id)
        {
            try
            {
                var response = await _developerService.GetDeveloperByIdAsync(id);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<DeveloperDto?, DevelopersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeveloper(int id)
        {
            try
            {
                var response = await _developerService.DeleteDeveloperAsync(id);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, DevelopersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeveloper([FromBody] DeveloperViewModel developerView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.AllErrors();
                    return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
                }

                var response = await _developerService.CreateDeveloperAsync(developerView);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return CreatedAtAction(nameof(GetDeveloperById), new { id = response.Data?.Id }, response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, DevelopersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeveloper(int id, [FromBody] DeveloperViewModel developerView)
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

                var response = await _developerService.UpdateDeveloperAsync(id, developerView);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<DeveloperDto?, DevelopersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }
    }
}
