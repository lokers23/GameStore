﻿using GameStore.API.Extensions;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.ViewModels.Publisher;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;
        private readonly ILogger<PublishersController> _logger;
        public PublishersController(IPublisherService publisherService, ILogger<PublishersController> logger)
        {
            _publisherService = publisherService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPublishers([FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string? name)
        {
            try
            {
                var response = await _publisherService.GetPublishersAsync(page, pageSize, name);
                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<Publisher>?, PublishersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublisherById(int id)
        {
            try
            {
                var response = await _publisherService.GetPublisherByIdAsync(id);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Publisher?, PublishersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(nameof(AccessRole.Moderator))]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            try
            {
                var response = await _publisherService.DeletePublisherAsync(id);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, PublishersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPost]
        [Authorize(nameof(AccessRole.Moderator))]
        public async Task<IActionResult> CreatePublisher([FromBody] PublisherViewModel publisherView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.AllErrors();
                    return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
                }

                var response = await _publisherService.CreatePublisherAsync(publisherView);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return CreatedAtAction(nameof(GetPublisherById), new { id = response.Data?.Id }, response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, PublishersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPut("{id}")]
        [Authorize(nameof(AccessRole.Moderator))]
        public async Task<IActionResult> UpdatePublisher(int id, [FromBody] PublisherViewModel publisherView)
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

                var response = await _publisherService.UpdatePublisherAsync(id, publisherView);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Publisher?, PublishersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }
    }
}
