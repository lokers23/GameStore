using GameStore.Domain.Dto.Platform;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Platform;

namespace GameStore.Service.Interfaces
{
    public interface IPlatformService
    {
        Task<Response<PlatformDto?>> CreatePlatformAsync(PlatformViewModel platformView);
        Task<Response<PlatformDto?>> UpdatePlatformAsync(int id, PlatformViewModel platformView);
        Task<Response<List<PlatformDto>?>> GetPlatformsAsync();
        Task<Response<PlatformDto?>> GetPlatformByIdAsync(int id);
        Task<Response<bool?>> DeletePlatformAsync(int id);
        Task<Response<bool>> CheckExistAsync(PlatformViewModel platformView, int id);
    }
}
