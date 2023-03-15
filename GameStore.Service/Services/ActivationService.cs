using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Activation;
using GameStore.Domain.ViewModels.Developer;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services;

public class ActivationService: IActivationService
{
    private readonly IRepository<Activation> _activationRepository;
    private readonly ILogger<ActivationService> _logger;
    public ActivationService(ILogger<ActivationService> logger, IRepository<Activation> activationRepository)
    {
        _logger = logger;
        _activationRepository = activationRepository;
    }
    public async Task<Response<List<Activation>?>> GetActivationsAsync()
    {
        try
        {
            var response = new Response<List<Activation>?>()
            {
                Status = HttpStatusCode.Ok
            };

            var genre = await _activationRepository.GetAll().ToListAsync();
            response.Data = genre;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<Activation>?, ActivationService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<Activation?>> GetActivationByIdAsync(int id)
    {
        try
        {
            var response = new Response<Activation?>()
            {
                Status = HttpStatusCode.Ok
            };

            var activation = await _activationRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (activation == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            response.Data = activation;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<Activation?, ActivationService>(exception, _logger);
            return response;
        }
    }
    
    public async Task<Response<Activation?>> CreateActivationAsync(ActivationViewModel activationView)
    {
        try
        {
            var response = new Response<Activation?>();

            var responseExist = await CheckExistAsync(activationView);
            if (responseExist.Data == true)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;

                return response;
            }

            var activation = new Activation()
            {
                Name = activationView.Name
            };

            await _activationRepository.CreateAsync(activation);
            response.Status = HttpStatusCode.Created;
            response.Message = MessageResponse.SuccessCreatedGenre;
            response.Data = activation;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<Activation?, ActivationService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<Activation?>> UpdateActivationAsync(int id, ActivationViewModel activationView)
    {
        try
        {
            var response = new Response<Activation?>();
            var activation = await _activationRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (activation == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            var responseExist = await CheckExistAsync(activationView, activation);
            if (responseExist.Data == true)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;

                return response;
            }

            activation.Name = activationView.Name;
            await _activationRepository.UpdateAsync(activation);
            response.Data = activation;
            response.Status = HttpStatusCode.NoContent;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<Activation?, ActivationService>(exception, _logger);
            return response;
        }
    }
    
    public async Task<Response<bool?>> DeleteActivationAsync(int id)
    {
        try
        {
            var response = new Response<bool?>()
            {
                Status = HttpStatusCode.NoContent
            };

            var activation = await _activationRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (activation == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            await _activationRepository.DeleteAsync(activation);

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, ActivationService>(exception, _logger);
            return response;
        }
    }

    public async Task<Response<bool>> CheckExistAsync(ActivationViewModel activationView, Activation? activationDb = null)
    {
        var response = new Response<bool>()
        {
            Data = false,
            Errors = new Dictionary<string, string[]>()
        };

        bool isExistByName;
        if (activationDb == null)
        {
            isExistByName = await IsExistByNameAsync(activationView.Name);
        }
        else
        {
            isExistByName = await IsExistByNameAsync(activationView.Name, activationDb.Name);
        }

        if (isExistByName)
        {
            response.Status = HttpStatusCode.Conflict;
            response.Data = true;
            response.Errors.Add(activationView.Name, new[] { MessageError.EntityNameExist });
        }

        return response;
    }
    private async Task<bool> IsExistByNameAsync(string name)
    {
        return await _activationRepository.GetAll()
            .AnyAsync(x => x.Name.Equals(name));
    }
    private async Task<bool> IsExistByNameAsync(string name, string nameDb)
    {
        if (name.Equals(nameDb))
        {
            return false;
        }

        return await IsExistByNameAsync(name);
    }
}