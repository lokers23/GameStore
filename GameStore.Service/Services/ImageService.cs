using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services;

public class ImageService: IImageService
{
    private readonly IRepository<Image> _imageRepository;
    private readonly ILogger<ImageService> _logger;
    public ImageService(ILogger<ImageService> logger, IRepository<Image> imageRepository)
    {
        _logger = logger;
        _imageRepository = imageRepository;
    }
    public async Task<Response<List<Image>?>> GetImagesAsync()
    {
        try
        {
            var response = new Response<List<Image>?>()
            {
                Status = HttpStatusCode.Ok
            };

            var images = await _imageRepository.GetAll().ToListAsync();
            response.Data = images;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<Image>?, ImageService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<Image?>> GetImageByIdAsync(int id)
    {
        try
        {
            var response = new Response<Image?>()
            {
                Status = HttpStatusCode.Ok
            };

            var image = await _imageRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (image == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            response.Data = image;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<Image?, ImageService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<Image?>> CreateImageAsync(Image image)
    {
        try
        {
            var response = new Response<Image?>();

            // var responseExist = await CheckExistAsync(activationView);
            // if (responseExist.Data == true)
            // {
            //     response.Errors = responseExist.Errors;
            //     response.Status = responseExist.Status;
            //     response.Message = responseExist.Message;
            //
            //     return response;
            // }

            var activation = new Image()
            {
                //Name = activationView.Name
            };

            await _imageRepository.CreateAsync(activation);
            response.Status = HttpStatusCode.Created;
            response.Message = MessageResponse.SuccessCreatedGenre;
            response.Data = activation;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<Image?, ImageService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<Image?>> UpdateImageAsync(Image image)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<bool?>> DeleteImageAsync(int id)
    {
        try
        {
            var response = new Response<bool?>()
            {
                Status = HttpStatusCode.NoContent
            };

            var image = await _imageRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (image == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            await _imageRepository.DeleteAsync(image);

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, ImageService>(exception, _logger);
            return response;
        }
    }
}