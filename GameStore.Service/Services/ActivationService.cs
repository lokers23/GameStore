using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Activation;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Activation;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services;

public class ActivationService : IActivationService
{
    private readonly IRepository<Activation> _activationRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ActivationService> _logger;
    public ActivationService(ILogger<ActivationService> logger, IRepository<Activation> activationRepository,
        IMapper mapper)
    {
        _logger = logger;
        _activationRepository = activationRepository;
        _mapper = mapper;
    }
    public async Task<Response<List<ActivationDto>?>> GetActivationsAsync(int? page, int? pageSize, string? name)
    {
        try
        {
            var response = new Response<List<ActivationDto>?>();
            var activations =  _activationRepository.GetAll()
                .Where(genre => 
                    (string.IsNullOrEmpty(name) || genre.Name.StartsWith(name)));

            if (page.HasValue && pageSize.HasValue)
            {
                var totalDevelopers = await activations.CountAsync();
                var hasNextPage = totalDevelopers > page * pageSize;
                var hasPreviousPage = page > 1;
                
                activations =  activations
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
                
                response.HasPreviousPage = hasPreviousPage;
                response.HasNextPage = hasNextPage;
            }
            
            response.Data = await activations.Select(activation => _mapper.Map<ActivationDto>(activation))
                .ToListAsync();;
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<ActivationDto>?, ActivationService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<ActivationDto?>> GetActivationByIdAsync(int id)
    {
        try
        {
            var response = new Response<ActivationDto?>();
            var activation = await _activationRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (activation == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            response.Data = _mapper.Map<ActivationDto>(activation);
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<ActivationDto?, ActivationService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<ActivationDto?>> CreateActivationAsync(ActivationViewModel activationView)
    {
        try
        {
            var response = new Response<ActivationDto?>();
            var responseExist = await CheckExistAsync(activationView);
            if (responseExist.Data)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;
                return response;
            }

            var activation = _mapper.Map<Activation>(activationView);
            await _activationRepository.CreateAsync(activation);

            response.Status = HttpStatusCode.Created;
            response.Message = MessageResponse.SuccessCreatedGenre;
            response.Data = _mapper.Map<ActivationDto>(activation);
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<ActivationDto?, ActivationService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<ActivationDto?>> UpdateActivationAsync(int id, ActivationViewModel activationView)
    {
        try
        {
            var response = new Response<ActivationDto?>();
            var activation = await _activationRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (activation == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            var responseExist = await CheckExistAsync(activationView, id);
            if (responseExist.Data)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;
                return response;
            }

            activation.Name = activationView.Name;
            await _activationRepository.UpdateAsync(activation);

            response.Data = _mapper.Map<ActivationDto>(activation);
            response.Status = HttpStatusCode.NoContent;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<ActivationDto?, ActivationService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<bool?>> DeleteActivationAsync(int id)
    {
        try
        {
            var response = new Response<bool?>();

            var activation = await _activationRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (activation == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            await _activationRepository.DeleteAsync(activation);

            response.Status = HttpStatusCode.NoContent;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, ActivationService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<bool>> CheckExistAsync(ActivationViewModel activationView, int id = 0)
    {
        var response = new Response<bool>()
        {
            Data = false,
            Errors = new Dictionary<string, string[]>()
        };

        var isExist = await _activationRepository.GetAll().AnyAsync(m =>
            m.Id != id &&
            m.Name.Equals(activationView.Name));

        if (isExist)
        {
            response.Status = HttpStatusCode.Conflict;
            response.Data = true;
            response.Errors.Add(nameof(Activation), new[] { MessageError.EntityExists });
        }

        return response;
    }
}