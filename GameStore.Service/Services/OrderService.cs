using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Order;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services;

public class OrderService: IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly ILogger<OrderService> _logger;
    public OrderService(ILogger<OrderService> logger, IRepository<Order> orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    public async Task<Response<List<Order>?>> GetOrdersAsync()
    {
        try
        {
            var response = new Response<List<Order>?>()
            {
                Status = HttpStatusCode.Ok
            };

            var orders = await _orderRepository.GetAll().ToListAsync();
            response.Data = orders;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<Order>?, OrderService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<Order?>> GetOrderByIdAsync(int id)
    {
        try
        {
            var response = new Response<Order?>()
            {
                Status = HttpStatusCode.Ok
            };

            var order = await _orderRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            response.Data = order;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<Order?, OrderService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<Order?>> CreateOrderAsync(OrderViewModel orderView)
    {
        try
        {
            var response = new Response<Order?>();

            // var responseExist = await CheckExistAsync(orderView);
            // if (responseExist.Data == true)
            // {
            //     response.Errors = responseExist.Errors;
            //     response.Status = responseExist.Status;
            //     response.Message = responseExist.Message;
            //
            //     return response;
            // }

            var order = new Order()
            {
                
            };

            await _orderRepository.CreateAsync(order);
            response.Status = HttpStatusCode.Created;
            response.Message = MessageResponse.SuccessCreatedGenre;
            response.Data = order;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<Order?, OrderService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<Order?>> UpdateOrderAsync(int id, OrderViewModel orderView)
    {
        try
        {
            var response = new Response<Order?>();
            var order = await _orderRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            // var responseExist = await CheckExistAsync(activationView, activation);
            // if (responseExist.Data == true)
            // {
            //     response.Errors = responseExist.Errors;
            //     response.Status = responseExist.Status;
            //     response.Message = responseExist.Message;
            //
            //     return response;
            // }

            //order.Name = orderView.Name;
            await _orderRepository.UpdateAsync(order);
            response.Data = order;
            response.Status = HttpStatusCode.NoContent;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<Order?, OrderService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<bool?>> DeleteOrderAsync(int id)
    {
        try
        {
            var response = new Response<bool?>()
            {
                Status = HttpStatusCode.NoContent
            };

            var order = await _orderRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            await _orderRepository.DeleteAsync(order);

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, OrderService>(exception, _logger);
            return response;
        }
    }
}