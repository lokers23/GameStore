using GameStore.API.Extensions;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Key;
using GameStore.Domain.Helpers;
using GameStore.Domain.ViewModels.Key;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeysController : ControllerBase
    {
        private readonly IKeyService _keyService;
        private readonly ILogger<KeysController> _logger;
        public KeysController(IKeyService keyService, ILogger<KeysController> logger)
        {
            _keyService = keyService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetKeys(int? gameId = null)
        {
            try
            {
                var response = await _keyService.GetKeysAsync(gameId);
                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<KeyDto>?, KeysController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetKeyById(int id)
        {
            try
            {
                var response = await _keyService.GetKeyByIdAsync(id);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<KeyDto?, KeysController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKey(int id)
        {
            try
            {
                var response = await _keyService.DeleteKeyAsync(id);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, KeysController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateKey([FromBody] KeyViewModel keyViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.AllErrors();
                    return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
                }

                var response = await _keyService.CreateKeyAsync(keyViewModel);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                //return CreatedAtAction(nameof(GetPublisherById), new { id = response.Data?.Id }, response);
                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, KeysController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKey(int id, [FromBody] KeyViewModel keyViewModel)
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

                var response = await _keyService.UpdateKeyAsync(id, keyViewModel);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<KeyDto?, KeysController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }
    }
}
