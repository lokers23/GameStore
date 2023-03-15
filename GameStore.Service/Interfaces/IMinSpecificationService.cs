using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.MinimumSpecification;

namespace GameStore.Service.Interfaces;

public interface IMinSpecificationService
{
    Task<Response<List<MinimumSpecification>?>> GetMinSpecsAsync();
    Task<Response<MinimumSpecification?>> GetMinSpecByIdAsync(int id);
    Task<Response<MinimumSpecification?>> CreateMinSpecAsync(MinSpecificationViewModel minSpecView);
    Task<Response<MinimumSpecification?>> UpdateMinSpecAsync(int id, MinSpecificationViewModel minSpecView);
    Task<Response<bool?>> DeleteMinSpecAsync(int id);
    Task<Response<bool>> CheckExistAsync(MinSpecificationViewModel minSpecView, MinimumSpecification? minSpecDb);
}