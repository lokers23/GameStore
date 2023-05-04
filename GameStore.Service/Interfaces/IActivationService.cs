using GameStore.Domain.Dto.Activation;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Activation;

namespace GameStore.Service.Interfaces;

public interface IActivationService
{
    Task<Response<List<ActivationDto>?>> GetActivationsAsync();
    Task<Response<ActivationDto?>> GetActivationByIdAsync(int id);
    Task<Response<ActivationDto?>> CreateActivationAsync(ActivationViewModel activationViewModel);
    Task<Response<ActivationDto?>> UpdateActivationAsync(int id, ActivationViewModel activationViewModel);
    Task<Response<bool?>> DeleteActivationAsync(int id);
    Task<Response<bool>> CheckExistAsync(ActivationViewModel activationViewModel, int id);
}