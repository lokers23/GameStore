using System.Security.Claims;
using GameStore.API.Extensions;
using GameStore.API.Helpers;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Order;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IKeyService _keyService;
        private readonly IUserService _userService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, IKeyService keyService, IUserService userService,ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _keyService = keyService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var response = await _orderService.GetOrdersAsync();
                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<Order>?, OrderController>(exception, _logger);
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
                var response = Catcher.CatchError<Order?, OrderController>(exception, _logger);
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
                var response = Catcher.CatchError<bool?, OrderController>(exception, _logger);
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

                var claimsIndentity = User.Identity as ClaimsIdentity;
                var success = int.TryParse(claimsIndentity.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
                var isExists = await _userService.GetUserByIdAsync(userId);
                if (isExists.Status == HttpStatusCode.NotFound)
                {
                    return StatusCode((int)isExists.Status, isExists);
                }
                
                var response = await _orderService.CreateOrderAsync(orderView, userId);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                var isSuccess = await _keyService.MarkUsedKeysAsync(response.Data.Keys);
                return CreatedAtAction(nameof(GetOrderById), new { id = response.Data?.Id }, response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, OrderController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderViewModel orderView)
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
                
                var claimsIndentity = User.Identity as ClaimsIdentity;
                var success = int.TryParse(claimsIndentity.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId);
                var isExists = await _userService.GetUserByIdAsync(userId);
                
                if (isExists.Status == HttpStatusCode.NotFound)
                {
                    return StatusCode((int)isExists.Status, isExists);
                }
                
                var response = await _orderService.UpdateOrderAsync(id, orderView);
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                var isSuccess = await _keyService.MarkUsedKeysAsync(response.Data.Keys);
                return NoContent();
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Order?, OrderController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }
    }
}
