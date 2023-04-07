using GameStore.Domain.Dto.Publisher;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Developer;

namespace GameStore.Service.Interfaces
{
    public interface IDeveloperService
    {
        Task<Response<DeveloperDto?>> CreateDeveloperAsync(DeveloperViewModel developer);
        Task<Response<DeveloperDto?>> UpdateDeveloperAsync(int id, DeveloperViewModel developer);
        Task<Response<List<DeveloperDto>?>> GetDevelopersAsync();
        Task<Response<DeveloperDto?>> GetDeveloperByIdAsync(int id);
        Task<Response<bool?>> DeleteDeveloperAsync(int id);
        Task<Response<bool>> CheckExistAsync(DeveloperViewModel developerView, int id = 0);
    }
}
