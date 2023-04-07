using GameStore.Domain.Dto.Key;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Key;

namespace GameStore.Service.Interfaces;

public interface IKeyService
{
    Task<Response<List<KeyDto>?>> GetKeysAsync();
    Task<Response<KeyDto?>> GetKeyByIdAsync(int id);
    Task<Response<KeyDto?>> CreateKeyAsync(KeyViewModel keyViewModel);
    Task<Response<KeyDto?>> UpdateKeyAsync(int id, KeyViewModel keyViewModel);
    Task<Response<bool?>> DeleteKeyAsync(int id);
    
    Task<Response<bool>> CheckExistAsync(KeyViewModel keyViewModel, int id);
}