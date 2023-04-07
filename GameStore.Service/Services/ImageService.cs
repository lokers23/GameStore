using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Image;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Image;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services;

public class ImageService: IImageService
{
    private readonly IRepository<Image> _imageRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ImageService> _logger;
    public ImageService(ILogger<ImageService> logger, IRepository<Image> imageRepository, IMapper mapper)
    {
        _logger = logger;
        _imageRepository = imageRepository;
        _mapper = mapper;
    }
    public async Task<Response<List<ImageDto>?>> GetImagesAsync()
    {
        try
        {
            var response = new Response<List<ImageDto>?>();
            var images = await _imageRepository.GetAll()
                .Include(image => image.Game)
                .Select(image => _mapper.Map<ImageDto>(image))
                .ToListAsync();
            
            response.Data = images;
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<ImageDto>?, ImageService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<ImageDto?>> GetImageByIdAsync(int id)
    {
        try
        {
            var response = new Response<ImageDto?>();
            var image = await _imageRepository.GetAll()
                .Include(image => image.Game)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (image == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            response.Data = _mapper.Map<ImageDto>(image);
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<ImageDto?, ImageService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<ImageDto?>> CreateImageAsync(ImageViewModel imageViewModel)
    {
        try
        {
            var response = new Response<ImageDto?>();
            var responseExist = await CheckExistAsync(imageViewModel);
            if (responseExist.Data)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;
            
                return response;
            }

            var image = _mapper.Map<Image>(imageViewModel);
            await _imageRepository.CreateAsync(image);
            
            response.Status = HttpStatusCode.Created;
            response.Data = _mapper.Map<ImageDto>(image);
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<ImageDto?, ImageService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<ImageDto?>> UpdateImageAsync(int id, ImageViewModel imageViewModel)
    {
        try
        {
            var response = new Response<ImageDto?>();
            var image = await _imageRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (image == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            var responseExist = await CheckExistAsync(imageViewModel, id);
            if (responseExist.Data)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;
                return response;
            }

            image.Name = imageViewModel.Name;
            image.GameId = imageViewModel.GameId.Value;
            await _imageRepository.UpdateAsync(image);
            
            response.Data = _mapper.Map<ImageDto>(image);
            response.Status = HttpStatusCode.NoContent;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<ImageDto?, ImageService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<bool?>> DeleteImageAsync(int id)
    {
        try
        {
            var response = new Response<bool?>();
            var image = await _imageRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (image == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            await _imageRepository.DeleteAsync(image);

            response.Status = HttpStatusCode.NoContent;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, ImageService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<bool>> CheckExistAsync(ImageViewModel imageViewModel, int id = 0)
    {
        var response = new Response<bool>()
        {
            Data = false,
            Errors = new Dictionary<string, string[]>()
        };

        var isExist = await _imageRepository.GetAll().AnyAsync(m =>
            m.Id != id &&
            m.Name.Equals(imageViewModel.Name) && 
            m.GameId == imageViewModel.GameId);

        if (isExist)
        {
            response.Status = HttpStatusCode.Conflict;
            response.Data = true;
            response.Errors.Add(nameof(Image), new[] { MessageError.EntityExists });
        }

        return response;
    }
}