using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Order;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Order;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Key> _keyRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;
    public OrderService(ILogger<OrderService> logger, IRepository<Order> orderRepository,
        IRepository<Key> keyRepository, IMapper mapper)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _keyRepository = keyRepository;
        _mapper = mapper;
    }
    public async Task<Response<List<OrderDto>?>> GetOrdersAsync()
    {
        try
        {
            var response = new Response<List<OrderDto>?>();
            var orders = await _orderRepository.GetAll()
                .Include(order => order.User)
                .Include(order => order.KeyOrders)
                    .ThenInclude(keyOrder => keyOrder.Key)
                .Select(order => _mapper.Map<OrderDto>(order))
                .ToListAsync();

            response.Data = orders;
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<OrderDto>?, OrderService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<OrderDto?>> GetOrderByIdAsync(int id)
    {
        try
        {
            var response = new Response<OrderDto?>();
            var order = await _orderRepository.GetAll()
                .Include(order => order.User)
                .Include(order => order.KeyOrders)
                    .ThenInclude(keyOrder => keyOrder.Key)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            response.Data = _mapper.Map<OrderDto>(order);
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<OrderDto?, OrderService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<OrderDto?>> CreateOrderAsync(OrderViewModel orderView, User user)
    {
        try
        {
            var response = new Response<OrderDto?>();
            var order = new Order()
            {
                PayOn = DateTime.Now,
                UserId = user.Id,
            };

            var keys = new List<string?>();
            foreach (var gameId in orderView.GameIds)
            {

                var key = await _keyRepository.GetAll()
                    .Include(key => key.Game)
                    .FirstOrDefaultAsync(key =>
                        key.GameId == gameId &&
                        key.IsUsed == false &&
                        !keys.Contains(key.Value));

                if (key == null)
                {
                    break;
                }

                var keyOrder = new KeyOrder { KeyId = key.Id, Order = order };
                order.KeyOrders.Add(keyOrder);
                order.Amount += key.Game.Price;

                keys.Add(key.Value);
            }

            if (keys.Count != orderView.GameIds.Count)
            {
                response.Status = HttpStatusCode.Conflict;
                response.Errors = new Dictionary<string, string[]> { { "Key", new[] { "Не достаточно ключей" } } };
                return response;
            }

            if (order.Amount > user.Balance)
            {

            }

            await _orderRepository.CreateAsync(order);

            response.Status = HttpStatusCode.Created;
            response.Data = _mapper.Map<OrderDto>(order);
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<OrderDto?, OrderService>(exception, _logger);
            return response;
        }
    }

    //
    public async Task<Response<OrderDto?>> UpdateOrderAsync(int id, OrderViewModel orderView)
    {
        try
        {
            var response = new Response<OrderDto?>();
            var order = await _orderRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            //order.Name = orderView.Name;
            await _orderRepository.UpdateAsync(order);

            response.Data = _mapper.Map<OrderDto>(order);
            response.Status = HttpStatusCode.NoContent;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<OrderDto?, OrderService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<bool?>> DeleteOrderAsync(int id)
    {
        try
        {
            var response = new Response<bool?>();
            var order = await _orderRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            await _orderRepository.DeleteAsync(order);

            response.Status = HttpStatusCode.NoContent;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, OrderService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<bool>> CheckExistAsync(OrderViewModel activationView, int id)
    {
        throw new NotImplementedException();
    }
}