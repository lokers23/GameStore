using GameStore.API.Extensions;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Order;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Order;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GameStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IKeyService _keyService;
        private readonly IGameService _gameService;
        private readonly IUserService _userService;
        private readonly IBalanceService _balanceService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, IKeyService keyService,
            IGameService gameService, IUserService userService, IBalanceService balanceService,ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _keyService = keyService;
            _gameService = gameService;
            _userService = userService;
            _balanceService = balanceService;
            _logger = logger;
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetOrdersByUser([FromQuery]int? page, [FromQuery]int? pageSize)
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var success = int.TryParse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
                var user = await _userService.GetUserByIdAsync(userId);
                if (user.Status == HttpStatusCode.NotFound)
                {
                    return StatusCode((int)user.Status, user);
                }

                var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
                var response = await _orderService.GetOrdersAsync(page, pageSize, userId);
                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<OrderDto>?, OrdersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }
        [HttpGet]
        [Authorize(nameof(AccessRole.Moderator))]
        public async Task<IActionResult> GetOrders([FromQuery]int? page, [FromQuery]int? pageSize)
        {
            try
            {
                var response = await _orderService.GetOrdersAsync(page, pageSize);
                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<OrderDto>?, OrdersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var response = await _orderService.GetOrderByIdAsync(id);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Order?, OrdersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var response = await _orderService.DeleteOrderAsync(id);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return NoContent();
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, OrdersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderViewModel orderView)
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

                var response = await _orderService.CreateOrderAsync(orderView, user.Data);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                var isSuccess = await _keyService.MarkUsedKeysAsync(response.Data.Keys);
                return CreatedAtAction(nameof(GetOrderById), new { id = response.Data?.Id }, response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, OrdersController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderViewModel orderView)
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
        //         var claimsIndentity = User.Identity as ClaimsIdentity;
        //         var success = int.TryParse(claimsIndentity.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
        //         var isExists = await _userService.GetUserByIdAsync(userId);
        //         
        //         if (isExists.Status == HttpStatusCode.NotFound)
        //         {
        //             return StatusCode((int)isExists.Status, isExists);
        //         }
        //         
        //         var response = await _orderService.UpdateOrderAsync(id, orderView);
        //         if ((int)response.Status >= 300)
        //         {
        //             return StatusCode((int)response.Status, response);
        //         }
        //
        //         var isSuccess = await _keyService.MarkUsedKeysAsync(response.Data.Keys);
        //         return NoContent();
        //     }
        //     catch (Exception exception)
        //     {
        //         var response = Catcher.CatchError<Order?, OrderController>(exception, _logger);
        //         return StatusCode((int)response.Status, response);
        //     }
        // }
    }
}
