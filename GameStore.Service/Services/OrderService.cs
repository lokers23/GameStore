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
using System.Linq;

namespace GameStore.Service.Services;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Key> _keyRepository;
    private readonly IRepository<Game> _gameRepository;
    private readonly IBalanceService _balanceService;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;
    public OrderService(ILogger<OrderService> logger, IRepository<Order> orderRepository,
        IRepository<Key> keyRepository, IRepository<Game> gameRepository, IBalanceService balanceService, IMapper mapper)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _keyRepository = keyRepository;
        _gameRepository = gameRepository;
        _balanceService = balanceService;
        _mapper = mapper;
    }
    public async Task<Response<List<OrderDto>?>> GetOrdersAsync(int? page, int? pageSize, string? login, int? userId = null)
    {
        try
        {
            var response = new Response<List<OrderDto>?>();
            var orders = _orderRepository.GetAll()
                .Include(order => order.User)
                .Include(order => order.KeyOrders)
                .ThenInclude(keyOrder => keyOrder.Key)
                .ThenInclude(key => key.Game)
                .Where(order => 
                    (!userId.HasValue || order.User.Id == userId) &&
                    (string.IsNullOrEmpty(login) || order.User.Login.StartsWith(login))
                    );
                
            if (page.HasValue && pageSize.HasValue)
            {
                var totalGames = await orders.CountAsync();
                var hasNextPage = totalGames > page * pageSize;
                var hasPreviousPage = page > 1;
                
                orders =  orders
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

                response.HasPreviousPage = hasPreviousPage;
                response.HasNextPage = hasNextPage;
            }
            
            response.Data = await orders
                .Select(order => _mapper.Map<OrderDto>(order))
                .ToListAsync();;
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

            var errors = new List<string>();
            for (int i = 0; i < orderView.GameCounts.Count; i++)
            {
                var gameCount = orderView.GameCounts[i];
                var game = await _gameRepository.GetAll()
                    .Include(game => game.Keys)
                    .FirstOrDefaultAsync(game => game.Id == gameCount.Id);

                if (game == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Errors = new Dictionary<string, string[]> { { "Game", new[] { $"Такой игры с id равной {gameCount.Id} нет" } } };
                    return response;
                }

                var notUsedKeys = game.Keys.Where(key => !key.IsUsed);
                var countNotUsedKeys= notUsedKeys.Count();
                if (countNotUsedKeys < gameCount.Count)
                {
                    response.Status = HttpStatusCode.Conflict;
                    string[] error = countNotUsedKeys != 0 ? 
                        new string[] { $"Для игры {game.Name} не хватает ключей. Максимум доступно ключей:{countNotUsedKeys}" } :
                        new string[] { $"В данный момент ключей у игры {game.Name} нет в наличии" };
                    response.Errors = new Dictionary<string, string[]> { { "Game", error } };
                    return response;
                }

                var keysForOrder = notUsedKeys.Take(gameCount.Count);
                foreach (var key in keysForOrder)
                {
                    var keyOrder = new KeyOrder { KeyId = key.Id, Order = order };
                    order.KeyOrders.Add(keyOrder);
                }

                order.Amount += game.Price * gameCount.Count;
                
            }

            //var keys = new List<string?>();
            //foreach (var gameId in orderView.GameIds)
            //{

            //    var key = await _keyRepository.GetAll()
            //        .Include(key => key.Game)
            //        .FirstOrDefaultAsync(key =>
            //            key.GameId == gameId &&
            //            key.IsUsed == false &&
            //            !keys.Contains(key.Value));

            //    if (key == null)
            //    {
            //        break;
            //    }

            //    var keyOrder = new KeyOrder { KeyId = key.Id, Order = order };
            //    order.KeyOrders.Add(keyOrder);
            //    order.Amount += key.Game.Price;

            //    keys.Add(key.Value);
            //}

            //if (keys.Count != orderView.GameIds.Count)
            //{
            //    response.Status = HttpStatusCode.Conflict;
            //    response.Errors = new Dictionary<string, string[]> { { "Key", new[] { "Не достаточно ключей" } } };
            //    return response;
            //}

            if (order.Amount > user.Balance)
            {
                response.Status = HttpStatusCode.Conflict;
                response.Errors = new Dictionary<string, string[]> { { "balance", new[] { "Не достаточно денег на балансе" } } };
                return response;
            }

            
            await _orderRepository.CreateAsync(order);
            await _balanceService.DeductFromBalance(user.Id, order.Amount);
            
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