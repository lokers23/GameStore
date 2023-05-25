using GameStore.Domain.Dto.Key;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Key;

namespace GameStore.Service.Interfaces;

public interface IKeyService
{
    Task<Response<List<KeyDto>?>> GetKeysAsync(int? page, int? pageSize, string? gameName, int? gameId = null);
    Task<Response<KeyDto?>> GetKeyByIdAsync(int id);
    Task<Response<KeyDto?>> CreateKeyAsync(KeyViewModel keyViewModel);
    Task<Response<KeyDto?>> UpdateKeyAsync(int id, KeyViewModel keyViewModel);
    Task<Response<bool?>> DeleteKeyAsync(int id);
    Task<bool> MarkUsedKeysAsync(List<KeyDto> keys);
    Task<Response<int?>> GetNumberOfKeys(int gameId);
    Task<Response<bool>> CheckExistAsync(KeyViewModel keyViewModel, int id);
}