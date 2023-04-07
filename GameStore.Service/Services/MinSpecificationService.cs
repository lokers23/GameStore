using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.MinimumSpecification;
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
    private readonly IMapper _mapper;
    public MinSpecificationService(ILogger<MinSpecificationService> logger, 
        IRepository<MinimumSpecification> minSpecRepository, IMapper mapper)
    {
        _logger = logger;
        _minSpecRepository = minSpecRepository;
        _mapper = mapper;
    }
    public async Task<Response<List<MinSpecDto>?>> GetMinSpecsAsync()
    {
        try
        {
            var response = new Response<List<MinSpecDto>?>()
            {
                Status = HttpStatusCode.Ok
            };

            var minSpecifications = await _minSpecRepository.GetAll()
                .Include(m => m.Platform)
                .Select(minSpecification => _mapper.Map<MinSpecDto>(minSpecification))
                .ToListAsync();
            
            response.Data = minSpecifications;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<MinSpecDto>?, MinSpecificationService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<MinSpecDto?>> GetMinSpecByIdAsync(int id)
    {
        try
        {
            var response = new Response<MinSpecDto?>()
            {
                Status = HttpStatusCode.Ok
            };

            var minSpecification = await _minSpecRepository.GetAll()
                .Include(x => x.Platform)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (minSpecification == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }
            
            response.Data = _mapper.Map<MinSpecDto>(minSpecification);
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<MinSpecDto?, MinSpecificationService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<MinSpecDto?>> CreateMinSpecAsync(MinSpecificationViewModel minSpecView)
    {
        try
        {
            var response = new Response<MinSpecDto?>();

            var responseExist = await CheckExistAsync(minSpecView);
            if (responseExist.Data)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;
            
                return response;
            }

            var minSpec = _mapper.Map<MinimumSpecification>(minSpecView);
            await _minSpecRepository.CreateAsync(minSpec);
            
            response.Status = HttpStatusCode.Created;
            response.Message = MessageResponse.SuccessCreatedGenre;
            response.Data = _mapper.Map<MinSpecDto>(minSpec);

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<MinSpecDto?, MinSpecificationService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<MinSpecDto?>> UpdateMinSpecAsync(int id, MinSpecificationViewModel minSpecView)
    {
        try
        {
            var response = new Response<MinSpecDto?>();
            
            var minSpec = await _minSpecRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (minSpec == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            var responseExist = await CheckExistAsync(minSpecView, id);
            if (responseExist.Data)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;
            
                return response;
            }
            
            minSpec.OperatingSystem = minSpecView.OperatingSystem;
            minSpec.Processor = minSpecView.Processor;
            minSpec.Memory = minSpecView.Memory;
            minSpec.Storage = minSpecView.Storage;
            minSpec.Graphics = minSpecView.Graphics;
            minSpec.PlatformId = minSpecView.PlatformId!.Value;
            
            await _minSpecRepository.UpdateAsync(minSpec);
            
            response.Data = _mapper.Map<MinSpecDto>(minSpec);
            response.Status = HttpStatusCode.NoContent;
            
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<MinSpecDto?, MinSpecificationService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<bool?>> DeleteMinSpecAsync(int id)
    {
        try
        {
            var response = new Response<bool?>();

            var genre = await _minSpecRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            await _minSpecRepository.DeleteAsync(genre);
            response.Status = HttpStatusCode.NoContent;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, MinSpecificationService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<bool>> CheckExistAsync(MinSpecificationViewModel minSpecView, int id = 0)
    {
        var response = new Response<bool>()
        {
            Data = false,
            Errors = new Dictionary<string, string[]>()
        };

        var isExist = await _minSpecRepository.GetAll().AnyAsync(m => 
                m.Id != id &&
                m.OperatingSystem.Equals(minSpecView.OperatingSystem) &&
                m.Processor.Equals(minSpecView.Processor) &&
                m.Memory.Equals(minSpecView.Memory) &&
                m.Storage.Equals(minSpecView.Storage) &&
                m.Graphics.Equals(minSpecView.Graphics) &&
                m.PlatformId == minSpecView.PlatformId);
        
        if (isExist)
        {
            response.Status = HttpStatusCode.Conflict;
            response.Data = true;
            response.Errors.Add(nameof(MinimumSpecification), new[] { MessageError.EntityExists });
        }

        return response;
    }
}