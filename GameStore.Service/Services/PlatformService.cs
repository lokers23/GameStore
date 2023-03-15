using GameStore.DAL.Interfaces;
using GameStore.DAL.Repositories;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Platform;
using GameStore.Domain.ViewModels.Publisher;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IRepository<Platform> _platformRepository;
        private readonly ILogger<PlatformService> _logger;
        public PlatformService(ILogger<PlatformService> logger, IRepository<Platform> platformRepository)
        {
            _logger = logger;
            _platformRepository = platformRepository;
        }
        public async Task<Response<List<Platform>?>> GetPlatformsAsync()
        {
            try
            {
                var response = new Response<List<Platform>?>()
                {
                    Status = HttpStatusCode.Ok
                };

                var platforms = await _platformRepository.GetAll().ToListAsync();
                response.Data = platforms;

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<Platform>?, PlatformService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<Platform?>> GetPlatformByIdAsync(int id)
        {
            try
            {
                var response = new Response<Platform?>()
                {
                    Status = HttpStatusCode.Ok
                };

                var platform = await _platformRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (platform == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundGenre;
                    return response;
                }

                response.Data = platform;

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Platform?, PlatformService>(exception, _logger);
                return response;
            }
        }

        public async Task<Response<Platform?>> CreatePlatformAsync(PlatformViewModel platformView)
        {
            try
            {
                var response = new Response<Platform?>();

                var responseExist = await CheckExistAsync(platformView);
                if (responseExist.Data == true)
                {
                    response.Errors = responseExist.Errors;
                    response.Status = responseExist.Status;
                    response.Message = responseExist.Message;

                    return response;
                }

                var platform = new Platform()
                {
                    Name = platformView.Name
                };

                await _platformRepository.CreateAsync(platform);
                response.Status = HttpStatusCode.Created;
                response.Message = MessageResponse.SuccessCreatedGenre;
                response.Data = platform;

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Platform?, PlatformService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<Platform?>> UpdatePlatformAsync(int id, PlatformViewModel platformView)
        {
            try
            {
                var response = new Response<Platform?>();
                var platform = await _platformRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (platform == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundGenre;
                    return response;
                }

                var responseExist = await CheckExistAsync(platformView, platform);
                if (responseExist.Data == true)
                {
                    response.Errors = responseExist.Errors;
                    response.Status = responseExist.Status;
                    response.Message = responseExist.Message;

                    return response;
                }

                platform.Name = platformView.Name;
                await _platformRepository.UpdateAsync(platform);
                response.Data = platform;
                response.Status = HttpStatusCode.NoContent;

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Platform?, PlatformService>(exception, _logger);
                return response;
            }
        }

        public async Task<Response<bool?>> DeletePlatformAsync(int id)
        {
            try
            {
                var response = new Response<bool?>()
                {
                    Status = HttpStatusCode.NoContent
                };

                var platform = await _platformRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (platform == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundGenre;
                    return response;
                }

                await _platformRepository.DeleteAsync(platform);

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, PlatformService>(exception, _logger);
                return response;
            }
        }

        public async Task<Response<bool>> CheckExistAsync(PlatformViewModel platformView, Platform? platformDb = null)
        {
            var response = new Response<bool>()
            {
                Data = false,
                Errors = new Dictionary<string, string[]>()
            };

            bool isExistByName;
            if (platformDb == null)
            {
                isExistByName = await IsExistByNameAsync(platformView.Name);
            }
            else
            {
                isExistByName = await IsExistByNameAsync(platformView.Name, platformDb.Name);
            }

            if (isExistByName)
            {
                response.Status = HttpStatusCode.Conflict;
                response.Data = true;
                response.Errors.Add(platformView.Name, new[] { MessageError.EntityNameExist });
            }

            return response;
        }
        private async Task<bool> IsExistByNameAsync(string name)
        {
            return await _platformRepository.GetAll()
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
}
