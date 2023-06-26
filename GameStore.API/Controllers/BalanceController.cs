using System.Security.Claims;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BalanceController: ControllerBase
{
    private readonly IBalanceService _balanceService;
    private readonly ILogger<UsersController> _logger;
    public BalanceController(IBalanceService balanceService,ILogger<UsersController> logger)
    {
        _balanceService = balanceService;
        _logger = logger;
    }

    [HttpPost("top-up")]
    [Authorize]
    public async Task<IActionResult> TopUpBalance(decimal amount)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var success = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
            if (!success) return Unauthorized();
            
            var response = await _balanceService.TopUpBalance(userId, amount);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }
            
            return Ok(response);
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, UsersController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }
    
    [HttpPost("deduct")]
    [Authorize]
    public async Task<IActionResult> DeductFromBalance(decimal amount)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var success = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
            if (!success) return Unauthorized();
            
            var response = await _balanceService.DeductFromBalance(userId, amount);
            if ((int)response.Status >= 300)
            {
                return StatusCode((int)response.Status, response);
            }
            
            return Ok(response);

        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, UsersController>(exception, _logger);
            return StatusCode((int)response.Status, response);
        }
    }
}