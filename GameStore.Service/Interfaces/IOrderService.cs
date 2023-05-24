using GameStore.Domain.Dto.Order;
using GameStore.Domain.Dto.User;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Activation;
using GameStore.Domain.ViewModels.Order;

namespace GameStore.Service.Interfaces;

public interface IOrderService
{
    Task<Response<List<OrderDto>?>> GetOrdersAsync(int? page, int? pageSize, int? userId = null);
    Task<Response<OrderDto?>> GetOrderByIdAsync(int id);
    Task<Response<OrderDto?>> CreateOrderAsync(OrderViewModel orderView, User user);
    Task<Response<OrderDto?>> UpdateOrderAsync(int id, OrderViewModel orderView);
    Task<Response<bool?>> DeleteOrderAsync(int id);
    Task<Response<bool>> CheckExistAsync(OrderViewModel activationView, int id);
}