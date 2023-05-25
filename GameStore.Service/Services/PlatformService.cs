using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Repositories;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Platform;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Platform;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IRepository<Platform> _platformRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformService> _logger;
        public PlatformService(ILogger<PlatformService> logger, IRepository<Platform> platformRepository, IMapper mapper)
        {
            _logger = logger;
            _platformRepository = platformRepository;
            _mapper = mapper;
        }
        public async Task<Response<List<PlatformDto>?>> GetPlatformsAsync(int? page, int? pageSize, string? name)
        {
            try
            {
                var response = new Response<List<PlatformDto>?>();
                var platforms = _platformRepository.GetAll()
                    .Where(genre => 
                        (string.IsNullOrEmpty(name) || genre.Name.StartsWith(name)));
                
                if (page.HasValue && pageSize.HasValue)
                {
                    var totalDevelopers = await platforms.CountAsync();
                    var hasNextPage = totalDevelopers > page * pageSize;
                    var hasPreviousPage = page > 1;
                
                    platforms =  platforms
                        .Skip((page.Value - 1) * pageSize.Value)
                        .Take(pageSize.Value);
                    
                    response.HasPreviousPage = hasPreviousPage;
                    response.HasNextPage = hasNextPage;
                }
                
                response.Status = HttpStatusCode.Ok;
                response.Data = await platforms
                    .Select(p => _mapper.Map<PlatformDto>(p))
                    .ToListAsync();

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<PlatformDto>?, PlatformService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<PlatformDto?>> GetPlatformByIdAsync(int id)
        {
            try
            {
                var response = new Response<PlatformDto?>()
                {
                    
                };

                var platform = await _platformRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (platform == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundGenre;
                    return response;
                }

                response.Status = HttpStatusCode.Ok;
                response.Data = _mapper.Map<PlatformDto>(platform);

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<PlatformDto?, PlatformService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<PlatformDto?>> CreatePlatformAsync(PlatformViewModel platformView)
        {
            try
            {
                var response = new Response<PlatformDto?>();
                var responseExist = await CheckExistAsync(platformView);
                
                if (responseExist.Data)
                {
                    response.Errors = responseExist.Errors;
                    response.Status = responseExist.Status;
                    response.Message = responseExist.Message;

                    return response;
                }

                var platform = _mapper.Map<Platform>(platformView);
                await _platformRepository.CreateAsync(platform);
                
                response.Status = HttpStatusCode.Created;
                response.Message = MessageResponse.SuccessCreatedGenre;
                response.Data = _mapper.Map<PlatformDto>(platform);

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<PlatformDto?, PlatformService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<PlatformDto?>> UpdatePlatformAsync(int id, PlatformViewModel platformView)
        {
            try
            {
                var response = new Response<PlatformDto?>();
                var platform = await _platformRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (platform == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundGenre;
                    return response;
                }

                var responseExist = await CheckExistAsync(platformView, id);
                if (responseExist.Data)
                {
                    response.Errors = responseExist.Errors;
                    response.Status = responseExist.Status;
                    response.Message = responseExist.Message;

                    return response;
                }

                platform.Name = platformView.Name;
                await _platformRepository.UpdateAsync(platform);
                
                response.Data = _mapper.Map<PlatformDto>(platform);
                response.Status = HttpStatusCode.NoContent;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<PlatformDto?, PlatformService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool?>> DeletePlatformAsync(int id)
        {
            try
            {
                var response = new Response<bool?>();

                var platform = await _platformRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (platform == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundGenre;
                    return response;
                }

                await _platformRepository.DeleteAsync(platform);

                response.Status = HttpStatusCode.NoContent;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, PlatformService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool>> CheckExistAsync(PlatformViewModel platformView, int id = 0)
        {
            var response = new Response<bool>()
            {
                Data = false,
                Errors = new Dictionary<string, string[]>()
            };

            var isExist = await _platformRepository.GetAll().AnyAsync(m => 
                m.Id != id &&
                m.Name.Equals(platformView.Name));
        
            if (isExist)
            {
                response.Status = HttpStatusCode.Conflict;
                response.Data = true;
                response.Errors.Add(nameof(Platform), new[] { MessageError.EntityExists });
            }

            return response;
        }
    }
}
