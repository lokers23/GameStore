
using GameStore.Domain.Enums;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Account;

namespace GameStore.Service.Interfaces
{
    public interface IImageService
    {
        public Task<Response<List<Image>?>> GetImagesAsync();
        public Task<Response<Image?>> GetImageByIdAsync(int id);
        public Task<Response<Image?>> CreateImageAsync(Image image);
        public Task<Response<Image?>> UpdateImageAsync(Image image);
        public Task<Response<bool?>> DeleteImageAsync(int id);
    }
}
