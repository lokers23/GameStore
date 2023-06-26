using System.Security.Claims;
using GameStore.API.Extensions;
using GameStore.API.Helpers;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.ViewModels.Account;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(nameof(AccessRole.Moderator))]
    public async Task<IActionResult> GetUsers([FromQuery] int? page,[FromQuery] int? pageSize,[FromQuery] string? login)
    {
        try
        {
            var response = await _userService.GetUsersAsync(page, pageSize, login);
            return Ok(response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<User>?, UsersController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpGet("{id}")]
    [Authorize(nameof(AccessRole.Moderator))]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var response = await _userService.GetUserByIdAsync(id);
            
            return Ok(response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<User>?, UsersController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(nameof(AccessRole.Administrator))]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var response = await _userService.DeleteUserAsync(id);
            if (response.Data = false)
            {
                return StatusCode((int)response.Status, response);
            }
            
            return Ok(response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<User>?, UsersController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }
    
    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetUserData()
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var success = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
            
            var response = await _userService.GetUserByIdAsync(userId);
            response.Data.Password = null;
            
            return StatusCode((int)response.Status, response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<User>?, UsersController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }

    [HttpPut("role")]
    [Authorize(nameof(AccessRole.Administrator))]
    public async Task<IActionResult> ChangeRole(ChangeRoleViewModel viewModel)
    {
        try
        {
            if (!Enum.IsDefined(typeof(AccessRole), viewModel.Role))
            {
                ModelState.AddModelError("role", "Не существует такой роли");
            }
            
            if (!ModelState.IsValid)
            {
                var errors = ModelState.AllErrors();
                return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
            }
            
            var response = await _userService.ChangeRoleUser(viewModel.Role.Value, viewModel.UserId.Value);
            if (!response.Data)
            {
                return StatusCode((int)response.Status, response);
            }
            
            return NoContent();
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool, UsersController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }
    
    [HttpPut("password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.AllErrors();
                return BadRequest(new { Message = MessageResponse.Invalid, Errors = errors });
            }
            
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var success = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
            var user = await _userService.GetUserByIdAsync(userId);
            if (user.Status == HttpStatusCode.NotFound)
            {
                return StatusCode((int)user.Status, user);
            }
            
            var hashPassword = AccountHelper.HashPassword(viewModel.NewPassword, user.Data.Login);
            
            var response = await _userService.ChangePassword(hashPassword, userId);
            if (!response.Data)
            {
                return StatusCode((int)response.Status, response);
            }
            
            return NoContent();
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool, UsersController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }
}