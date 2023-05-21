using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services;

public class BalanceService : IBalanceService
{
    private readonly ILogger<BalanceService> _logger;
    private readonly IRepository<User> _userRepository;

    public BalanceService(ILogger<BalanceService> logger, IRepository<User> userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }
    public async Task<Response<bool?>> TopUpBalance(int userId, decimal amount)
    {
        try
        {
            var response = new Response<bool?>();
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = "Такой пользователь не найден";
                return response;
            }

            user.Balance += amount;
            await _userRepository.UpdateAsync(user);
            
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, BalanceService>(exception, _logger);
            response.Data = false;
            return response;
        }
    }

    public async Task<Response<bool?>> DeductFromBalance(int userId, decimal amount)
    {
        try
        {
            var response = new Response<bool?>();
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = "Такой пользователь не найден";
                return response;
            }

            if (user.Balance < amount)
            {
                response.Status = HttpStatusCode.Conflict;
                response.Message = "На балансе не хватает средств";
                return response;
            }
            
            user.Balance -= amount;
            await _userRepository.UpdateAsync(user);
            
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, BalanceService>(exception, _logger);
            response.Data = false;
            return response;
        }
    }
}