using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.MinimumSpecification;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services;

public class MinSpecificationService: IMinSpecificationService
{
    private readonly IRepository<MinimumSpecification> _minSpecRepository;
    private readonly ILogger<MinSpecificationService> _logger;
    public MinSpecificationService(ILogger<MinSpecificationService> logger, IRepository<MinimumSpecification> minSpecRepository)
    {
        _logger = logger;
        _minSpecRepository = minSpecRepository;
    }
    public async Task<Response<List<MinimumSpecification>?>> GetMinSpecsAsync()
    {
        try
        {
            var response = new Response<List<MinimumSpecification>?>()
            {
                Status = HttpStatusCode.Ok
            };

            var minSpecifications = await _minSpecRepository.GetAll().ToListAsync();
            response.Data = minSpecifications;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<MinimumSpecification>?, MinSpecificationService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<MinimumSpecification?>> GetMinSpecByIdAsync(int id)
    {
        try
        {
            var response = new Response<MinimumSpecification?>()
            {
                Status = HttpStatusCode.Ok
            };

            var genre = await _minSpecRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            response.Data = genre;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<MinimumSpecification?, MinSpecificationService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<MinimumSpecification?>> CreateMinSpecAsync(MinSpecificationViewModel minSpecView)
    {
        try
        {
            var response = new Response<MinimumSpecification?>();

            // var responseExist = await CheckExistAsync(minSpecView);
            // if (responseExist.Data)
            // {
            //     response.Errors = responseExist.Errors;
            //     response.Status = responseExist.Status;
            //     response.Message = responseExist.Message;
            //
            //     return response;
            // }

           
            var minSpec = new MinimumSpecification()
            {
                OperatingSystem = minSpecView.OperatingSystem,
                Processor = minSpecView.Processor,
                Memory = minSpecView.Memory,
                Storage = minSpecView.Storage,
                Graphics = minSpecView.Graphics,
                PlatformId = minSpecView.PlatformId!.Value
            };

            await _minSpecRepository.CreateAsync(minSpec);
            response.Status = HttpStatusCode.Created;
            response.Message = MessageResponse.SuccessCreatedGenre;
            response.Data = minSpec;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<MinimumSpecification?, MinSpecificationService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<MinimumSpecification?>> UpdateMinSpecAsync(int id, MinSpecificationViewModel minSpecView)
    {
        try
        {
            var response = new Response<MinimumSpecification?>();
            var minSpec = await _minSpecRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (minSpec == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            // var responseExist = await CheckExistAsync(minSpecView, minSpec);
            // if (responseExist.Data)
            // {
            //     response.Errors = responseExist.Errors;
            //     response.Status = responseExist.Status;
            //     response.Message = responseExist.Message;
            //
            //     return response;
            // }
            
            minSpec.OperatingSystem = minSpecView.OperatingSystem;
            minSpec.Processor = minSpecView.Processor;
            minSpec.Memory = minSpecView.Memory;
            minSpec.Storage = minSpecView.Storage;
            minSpec.Graphics = minSpecView.Graphics;
            minSpec.PlatformId = minSpecView.PlatformId!.Value;
            
            await _minSpecRepository.UpdateAsync(minSpec);
            response.Data = minSpec;
            response.Status = HttpStatusCode.NoContent;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<MinimumSpecification?, MinSpecificationService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<bool?>> DeleteMinSpecAsync(int id)
    {
        try
        {
            var response = new Response<bool?>()
            {
                Status = HttpStatusCode.NoContent
            };

            var genre = await _minSpecRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            await _minSpecRepository.DeleteAsync(genre);

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, MinSpecificationService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<bool>> CheckExistAsync(MinSpecificationViewModel minSpecView, MinimumSpecification? minSpecDb = null)
    {
        throw new NotImplementedException();
    }
}