
using GameStore.Domain.Dto.Image;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Image;

namespace GameStore.Service.Interfaces
{
    public interface IImageService
    {
        public Task<Response<List<ImageDto>?>> GetImagesAsync(int gameId);
        public Task<Response<ImageDto?>> GetImageByIdAsync(int id);
        public Task<Response<ImageDto?>> CreateImageAsync(ImageViewModel imageViewModel);
        public Task<Response<ImageDto?>> UpdateImageAsync(int id, ImageViewModel imageViewModel);
        public Task<Response<bool?>> DeleteImageAsync(int id);
        public Task<Response<bool>> CheckExistAsync(ImageViewModel imageViewModelView, int id);
    }
}
