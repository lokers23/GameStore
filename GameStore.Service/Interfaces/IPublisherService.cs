using GameStore.Domain.Dto.Publisher;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Publisher;

namespace GameStore.Service.Interfaces
{
    public interface IPublisherService
    {
        Task<Response<PublisherDto?>> CreatePublisherAsync(PublisherViewModel publisher);
        Task<Response<PublisherDto?>> UpdatePublisherAsync(int id, PublisherViewModel publisher);
        Task<Response<List<PublisherDto>?>> GetPublishersAsync(int? page, int? pageSize, string? name);
        Task<Response<PublisherDto?>> GetPublisherByIdAsync(int id);
        Task<Response<bool?>> DeletePublisherAsync(int id);
        Task<Response<bool>> CheckExistAsync(PublisherViewModel publisherView, int id);
    }
}
