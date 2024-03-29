﻿using GameStore.API.Extensions;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.ViewModels.Genre;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;
    private readonly ILogger<GenresController> _logger;
    public GenresController(IGenreService genreService, ILogger<GenresController> logger)
    {
        _genreService = genreService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetGenres([FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string? name)
    {
        try
        {
            var response = await _genreService.GetGenresAsync(page, pageSize, name);
            return Ok(response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<Genre>?, GenresController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenreById(int id)
    {
        try
        {
            var response = await _genreService.GetGenreByIdAsync(id);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            return Ok(response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<Genre?, GenresController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(nameof(AccessRole.Moderator))]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        try
        {
            var response = await _genreService.DeleteGenreAsync(id);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            return NoContent();
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, GenresController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpPost]
    [Authorize(nameof(AccessRole.Moderator))]
    public async Task<IActionResult> CreateGenre([FromBody] GenreViewModel genreViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.AllErrors();
                return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
            }

            var response = await _genreService.CreateGenreAsync(genreViewModel);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            return CreatedAtAction(nameof(GetGenreById), new { id = response.Data?.Id }, response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, GenresController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpPut("{id}")]
    [Authorize(nameof(AccessRole.Moderator))]
    public async Task<IActionResult> UpdateGenre(int id, [FromBody] GenreViewModel genreViewModel)
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

            var response = await _genreService.UpdateGenreAsync(id, genreViewModel);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }

            return NoContent();
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<Genre?, GenresController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }
}