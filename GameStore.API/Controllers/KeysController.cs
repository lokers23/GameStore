using GameStore.API.Extensions;
using GameStore.Domain.Constants;
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

        // [HttpGet]
        // public async Task<IActionResult> GetPublishers()
        // {
        //     try
        //     {
        //         var response = await _publisherService.GetPublishersAsync();
        //         return Ok(response);
        //     }
        //     catch (Exception exception)
        //     {
        //         var response = Catcher.CatchError<List<Publisher>?, PublishersController>(exception, _logger);
        //         return StatusCode((int)response.Status, response);
        //     }
        // }
        //
        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetPublisherById(int id)
        // {
        //     try
        //     {
        //         var response = await _publisherService.GetPublisherByIdAsync(id);
        //         if ((int)response.Status >= 300)
        //         {
        //             return StatusCode((int)response.Status, response);
        //         }
        //
        //         return Ok(response);
        //     }
        //     catch (Exception exception)
        //     {
        //         var response = Catcher.CatchError<Publisher?, PublishersController>(exception, _logger);
        //         return StatusCode((int)response.Status, response);
        //     }
        // }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeletePublisher(int id)
        // {
        //     try
        //     {
        //         var response = await _publisherService.DeletePublisherAsync(id);
        //         if ((int)response.Status >= 300)
        //         {
        //             return StatusCode((int)response.Status, response);
        //         }
        //
        //         return NoContent();
        //     }
        //     catch (Exception exception)
        //     {
        //         var response = Catcher.CatchError<bool?, PublishersController>(exception, _logger);
        //         return StatusCode((int)response.Status, response);
        //     }
        // }

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

        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdatePublisher(int id, [FromBody] PublisherViewModel publisherView)
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
        //         var response = await _publisherService.UpdatePublisherAsync(id, publisherView);
        //         if ((int)response.Status >= 300)
        //         {
        //             return StatusCode((int)response.Status, response);
        //         }
        //
        //         return NoContent();
        //     }
        //     catch (Exception exception)
        //     {
        //         var response = Catcher.CatchError<Publisher?, PublishersController>(exception, _logger);
        //         return StatusCode((int)response.Status, response);
        //     }
        // }
    }
}
