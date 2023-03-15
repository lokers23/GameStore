using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Publisher;

namespace GameStore.Service.Interfaces
{
    public interface IPublisherService
    {
        Task<Response<Publisher?>> CreatePublisherAsync(PublisherViewModel publisher);
        Task<Response<Publisher?>> UpdatePublisherAsync(int id, PublisherViewModel publisher);
        Task<Response<List<Publisher>?>> GetPublishersAsync();
        Task<Response<Publisher?>> GetPublisherByIdAsync(int id);
        Task<Response<bool?>> DeletePublisherAsync(int id);
        Task<Response<bool>> CheckExistAsync(PublisherViewModel publisherView, Publisher? publisher);
    }
}
