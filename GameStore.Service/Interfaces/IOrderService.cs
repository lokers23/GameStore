using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Activation;
using GameStore.Domain.ViewModels.Order;

namespace GameStore.Service.Interfaces;

public interface IOrderService
{
    Task<Response<List<Order>?>> GetOrdersAsync();
    Task<Response<Order?>> GetOrderByIdAsync(int id);
    Task<Response<Order?>> CreateOrderAsync(OrderViewModel orderView);
    Task<Response<Order?>> UpdateOrderAsync(int id, OrderViewModel orderView);
    Task<Response<bool?>> DeleteOrderAsync(int id);
    //Task<Response<bool>> CheckExistAsync(ActivationViewModel activationView, Activation? activation);
}