using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Key;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Key;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services;

public class KeyService: IKeyService
{
    private readonly IRepository<Key> _keyRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<KeyService> _logger;
    public KeyService(ILogger<KeyService> logger, IRepository<Key> keyRepository, IMapper mapper)
    {
        _logger = logger;
        _keyRepository = keyRepository;
        _mapper = mapper;
    }
    public async Task<Response<List<KeyDto>?>> GetKeysAsync()
    {
        try
        {
            var response = new Response<List<KeyDto>?>();
            var keys = await _keyRepository.GetAll()
                .Include(key => key.Game)
                .Include(key => key.Activation)
                .Select(key => _mapper.Map<KeyDto>(key))
                .ToListAsync();
            
            response.Data = keys;
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<KeyDto>?, KeyService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<KeyDto?>> GetKeyByIdAsync(int id)
    {
        try
        {
            var response = new Response<KeyDto?>();
            var key = await _keyRepository.GetAll()
                .Include(key => key.Game)
                .Include(key => key.Activation)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (key == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            response.Data = _mapper.Map<KeyDto>(key);
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<KeyDto?, KeyService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<KeyDto?>> CreateKeyAsync(KeyViewModel keyViewModel)
    {
        try
        {
            var response = new Response<KeyDto?>();
            var responseExist = await CheckExistAsync(keyViewModel);
            if (responseExist.Data)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;
                return response;
            }

            var key = _mapper.Map<Key>(keyViewModel);
            await _keyRepository.CreateAsync(key);
            
            response.Status = HttpStatusCode.Created;
            response.Message = MessageResponse.SuccessCreatedGenre;
            response.Data = _mapper.Map<KeyDto>(key);
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<KeyDto?, KeyService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<KeyDto?>> UpdateKeyAsync(int id, KeyViewModel keyViewModel)
    {
        try
        {
            var response = new Response<KeyDto?>();
            var key = await _keyRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (key == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            var responseExist = await CheckExistAsync(keyViewModel, id);
            if (responseExist.Data)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;
                return response;
            }

            key.Value = keyViewModel.Value;
            key.ActivationId = keyViewModel.ActivationId;
            key.GameId = keyViewModel.GameId;
            key.IsUsed = keyViewModel.IsUsed!.Value;
            await _keyRepository.UpdateAsync(key);
            
            response.Data = _mapper.Map<KeyDto>(key);
            response.Status = HttpStatusCode.NoContent;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<KeyDto?, KeyService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<bool?>> DeleteKeyAsync(int id)
    {
        try
        {
            var response = new Response<bool?>();
            var activation = await _keyRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (activation == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            await _keyRepository.DeleteAsync(activation);

            response.Status = HttpStatusCode.NoContent;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, KeyService>(exception, _logger);
            return response;
        }
    }

    public async Task<bool> MarkUsedKeysAsync(List<KeyDto> keys)
    {
        try
        {
            //foreach (var key in keys.Select(keyDto => _mapper.Map<Key>(keyDto)))
            //{

            //    key.IsUsed = true;
            //    await _keyRepository.UpdateAsync(key);
            //}

            foreach (var keyDto in keys)
            {
                var key = await _keyRepository.GetAll().FirstOrDefaultAsync(key => key.Id == keyDto.Id);
                if (key != null)
                {
                    key.IsUsed = true;
                    await _keyRepository.UpdateAsync(key);
                }
            }

            return true;
        }
        catch (Exception exception)
        {
            return false;
        }
    }

    public async Task<Response<bool>> CheckExistAsync(KeyViewModel keyViewModel, int id = 0)
    {
        var response = new Response<bool>()
        {
            Data = false,
            Errors = new Dictionary<string, string[]>()
        };

        var isExist = await _keyRepository.GetAll().AnyAsync(m => 
            m.Id != id &&
            m.Value.Equals(keyViewModel.Value));
        
        if (isExist)
        {
            response.Status = HttpStatusCode.Conflict;
            response.Data = true;
            response.Errors.Add(nameof(Key), new[] { MessageError.EntityExists });
        }

        return response;
    }
}