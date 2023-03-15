using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Platform;
using GameStore.Domain.ViewModels.Publisher;

namespace GameStore.Service.Interfaces
{
    public interface IPlatformService
    {
        Task<Response<Platform?>> CreatePlatformAsync(PlatformViewModel platformView);
        Task<Response<Platform?>> UpdatePlatformAsync(int id, PlatformViewModel platformView);
        Task<Response<List<Platform>?>> GetPlatformsAsync();
        Task<Response<Platform?>> GetPlatformByIdAsync(int id);
        Task<Response<bool?>> DeletePlatformAsync(int id);
        Task<Response<bool>> CheckExistAsync(PlatformViewModel platformView, Platform? platformDb);
    }
}
