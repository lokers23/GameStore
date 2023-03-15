using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Activation;

namespace GameStore.Service.Interfaces;

public interface IActivationService
{
    Task<Response<List<Activation>?>> GetActivationsAsync();
    Task<Response<Activation?>> GetActivationByIdAsync(int id);
    Task<Response<Activation?>> CreateActivationAsync(ActivationViewModel activation);
    Task<Response<Activation?>> UpdateActivationAsync(int id, ActivationViewModel activation);
    Task<Response<bool?>> DeleteActivationAsync(int id);
    Task<Response<bool>> CheckExistAsync(ActivationViewModel activationView, Activation? activation);
}