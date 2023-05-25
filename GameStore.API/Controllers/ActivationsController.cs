using GameStore.API.Extensions;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Activation;
using GameStore.Domain.Helpers;
using GameStore.Domain.ViewModels.Activation;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivationsController : ControllerBase
    {
        private readonly IActivationService _activationService;
        private readonly ILogger<ActivationsController> _logger;
        public ActivationsController(IActivationService activationService, ILogger<ActivationsController> logger)
        {
            _activationService = activationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetActivations([FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string? name)
        {
            try
            {
                var response = await _activationService.GetActivationsAsync(page, pageSize, name);
                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<ActivationDto>?, ActivationsController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetActivationById(int id)
        {
            try
            {
                var response = await _activationService.GetActivationByIdAsync(id);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<ActivationDto?, ActivationsController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteActivation(int id)
        {
            try
            {
                var response = await _activationService.DeleteActivationAsync(id);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, ActivationsController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivation([FromBody] ActivationViewModel activationView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.AllErrors();
                    return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
                }

                var response = await _activationService.CreateActivationAsync(activationView);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return CreatedAtAction(nameof(GetActivationById), new { id = response.Data?.Id }, response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, ActivationsController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateActivation(int id, [FromBody] ActivationViewModel activationView)
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

                var response = await _activationService.UpdateActivationAsync(id, activationView);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<ActivationDto?, ActivationsController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }
    }
}
