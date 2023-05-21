using System.Security.Claims;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController: ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;
    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var response = await _userService.GetUsersAsync();
            return Ok(response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<User>?, UsersController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpDelete("id")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var response = await _userService.DeleteUserAsync(id);
            return Ok(response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<User>?, UsersController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }
    
    [HttpGet("profile")]
    public async Task<IActionResult> GetUserData()
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var success = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
            
            var response = await _userService.GetUserByIdAsync(userId);
            return StatusCode((int)response.Status, response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<User>?, UsersController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }
}