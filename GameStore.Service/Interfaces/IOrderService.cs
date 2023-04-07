using GameStore.Domain.Dto.Order;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Activation;
using GameStore.Domain.ViewModels.Order;

namespace GameStore.Service.Interfaces;

public interface IOrderService
{
    Task<Response<List<OrderDto>?>> GetOrdersAsync();
    Task<Response<OrderDto?>> GetOrderByIdAsync(int id);
    Task<Response<OrderDto?>> CreateOrderAsync(OrderViewModel orderView);
    Task<Response<OrderDto?>> UpdateOrderAsync(int id, OrderViewModel orderView);
    Task<Response<bool?>> DeleteOrderAsync(int id);
    Task<Response<bool>> CheckExistAsync(OrderViewModel activationView, int id);
}