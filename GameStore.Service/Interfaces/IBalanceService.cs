using GameStore.Domain.Response;

namespace GameStore.Service.Interfaces;

public interface IBalanceService
{
    Task<Response<bool?>> TopUpBalance(int userId, decimal amount);
    Task<Response<bool?>> DeductFromBalance(int userId, decimal amount);

}