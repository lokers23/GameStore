using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Developer;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services
{
    public class DeveloperService : IDeveloperService
    {
        private readonly IRepository<Developer> _developerRepository;
        private readonly ILogger<DeveloperService> _logger;
        public DeveloperService(ILogger<DeveloperService> logger, IRepository<Developer> genreRepository)
        {
            _logger = logger;
            _developerRepository = genreRepository;
        }
        public async Task<Response<List<Developer>?>> GetDevelopersAsync()
        {
            try
            {
                var response = new Response<List<Developer>?>()
                {
                    Status = HttpStatusCode.Ok
                };

                var developer = await _developerRepository.GetAll().ToListAsync();
                response.Data = developer;

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<Developer>?, DeveloperService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<Developer?>> GetDeveloperByIdAsync(int id)
        {
            try
            {
                var response = new Response<Developer?>()
                {
                    Status = HttpStatusCode.Ok
                };

                var developer = await _developerRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (developer == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundGenre;
                    return response;
                }

                response.Data = developer;

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Developer?, DeveloperService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<Developer?>> CreateDeveloperAsync(DeveloperViewModel developerView)
        {
            try
            {
                var response = new Response<Developer?>();

                var responseExist = await CheckExistAsync(developerView);
                if (responseExist.Data == true)
                {
                    response.Errors = responseExist.Errors;
                    response.Status = responseExist.Status;
                    response.Message = responseExist.Message;

                    return response;
                }

                var developer = new Developer()
                {
                    Name = developerView.Name
                };

                await _developerRepository.CreateAsync(developer);
                response.Status = HttpStatusCode.Created;
                response.Message = MessageResponse.SuccessCreatedGenre;
                response.Data = developer;

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Developer?, DeveloperService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<Developer?>> UpdateDeveloperAsync(int id, DeveloperViewModel developerView)
        {
            try
            {
                var response = new Response<Developer?>();
                var developer = await _developerRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (developer == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundGenre;
                    return response;
                }

                var responseExist = await CheckExistAsync(developerView, developer);
                if (responseExist.Data == true)
                {
                    response.Errors = responseExist.Errors;
                    response.Status = responseExist.Status;
                    response.Message = responseExist.Message;

                    return response;
                }

                developer.Name = developerView.Name;
                await _developerRepository.UpdateAsync(developer);
                response.Data = developer;
                response.Status = HttpStatusCode.NoContent;

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Developer?, DeveloperService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool?>> DeleteDeveloperAsync(int id)
        {
            try
            {
                var response = new Response<bool?>()
                {
                    Status = HttpStatusCode.NoContent
                };

                var developer = await _developerRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (developer == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundGenre;
                    return response;
                }

                await _developerRepository.DeleteAsync(developer);

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, DeveloperService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool>> CheckExistAsync(DeveloperViewModel developerView, Developer? developerDb = null)
        {
            var response = new Response<bool>()
            {
                Data = false,
                Errors = new Dictionary<string, string[]>()
            };

            bool isExistByName;
            if (developerDb == null)
            {
                isExistByName = await IsExistByNameAsync(developerView.Name);
            }
            else
            {
                isExistByName = await IsExistByNameAsync(developerView.Name, developerDb.Name);
            }

            if (isExistByName)
            {
                response.Status = HttpStatusCode.Conflict;
                response.Data = true;
                response.Errors.Add(developerView.Name, new[] { MessageError.EntityNameExist });
            }

            return response;
        }
        private async Task<bool> IsExistByNameAsync(string name)
        {
            return await _developerRepository.GetAll()
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
