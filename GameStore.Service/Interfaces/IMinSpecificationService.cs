using GameStore.Domain.Dto.MinimumSpecification;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.MinimumSpecification;

namespace GameStore.Service.Interfaces;

public interface IMinSpecificationService
{
    Task<Response<List<MinSpecDto>?>> GetMinSpecsAsync(string? platfrom = null);
    Task<Response<MinSpecDto?>> GetMinSpecByIdAsync(int id);
    Task<Response<MinSpecDto?>> CreateMinSpecAsync(MinSpecificationViewModel minSpecView);
    Task<Response<MinSpecDto?>> UpdateMinSpecAsync(int id, MinSpecificationViewModel minSpecView);
    Task<Response<bool?>> DeleteMinSpecAsync(int id);
    Task<Response<bool>> CheckExistAsync(MinSpecificationViewModel minSpecView, int id);
}