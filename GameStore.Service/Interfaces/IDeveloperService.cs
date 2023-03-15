using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Developer;

namespace GameStore.Service.Interfaces
{
    public interface IDeveloperService
    {
        Task<Response<Developer?>> CreateDeveloperAsync(DeveloperViewModel developer);
        Task<Response<Developer?>> UpdateDeveloperAsync(int id, DeveloperViewModel developer);
        Task<Response<List<Developer>?>> GetDevelopersAsync();
        Task<Response<Developer?>> GetDeveloperByIdAsync(int id);
        Task<Response<bool?>> DeleteDeveloperAsync(int id);
        Task<Response<bool>> CheckExistAsync(DeveloperViewModel developerView, Developer? developer);
    }
}
