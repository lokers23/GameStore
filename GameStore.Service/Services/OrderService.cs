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

public class OrderService: IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;
    public OrderService(ILogger<OrderService> logger, IRepository<Order> orderRepository, IMapper mapper)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    public async Task<Response<List<OrderDto>?>> GetOrdersAsync()
    {
        try
        {
            var response = new Response<List<OrderDto>?>();
            var orders = await _orderRepository.GetAll()
                .Include(order => order.User)
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
    public async Task<Response<OrderDto?>> CreateOrderAsync(OrderViewModel orderView)
    {
        try
        {
            var response = new Response<OrderDto?>();

            // var responseExist = await CheckExistAsync(orderView);
            // if (responseExist.Data == true)
            // {
            //     response.Errors = responseExist.Errors;
            //     response.Status = responseExist.Status;
            //     response.Message = responseExist.Message;
            //
            //     return response;
            // }

            var order = _mapper.Map<Order>(orderView);
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