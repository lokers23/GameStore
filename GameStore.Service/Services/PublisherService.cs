using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Publisher;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.Service.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IRepository<Publisher> _publisherRepository;
        private readonly ILogger<PublisherService> _logger;
        public PublisherService(ILogger<PublisherService> logger, IRepository<Publisher> publisherRepository)
        {
            _logger = logger;
            _publisherRepository = publisherRepository;
        }
        public async Task<Response<List<Publisher>?>> GetPublishersAsync()
        {
            try
            {
                var response = new Response<List<Publisher>?>()
                {
                    Status = HttpStatusCode.Ok
                };

                var developer = await _publisherRepository.GetAll().ToListAsync();
                response.Data = developer;

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<Publisher>?, PublisherService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<Publisher?>> GetPublisherByIdAsync(int id)
        {
            try
            {
                var response = new Response<Publisher?>()
                {
                    Status = HttpStatusCode.Ok
                };

                var developer = await _publisherRepository.GetAll()
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
                var response = Catcher.CatchError<Publisher?, PublisherService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<Publisher?>> CreatePublisherAsync(PublisherViewModel publisherView)
        {
            try
            {
                var response = new Response<Publisher?>();

                var responseExist = await CheckExistAsync(publisherView);
                if (responseExist.Data == true)
                {
                    response.Errors = responseExist.Errors;
                    response.Status = responseExist.Status;
                    response.Message = responseExist.Message;

                    return response;
                }

                var publisher = new Publisher()
                {
                    Name = publisherView.Name
                };

                await _publisherRepository.CreateAsync(publisher);
                response.Status = HttpStatusCode.Created;
                response.Message = MessageResponse.SuccessCreatedGenre;
                response.Data = publisher;

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Publisher?, PublisherService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<Publisher?>> UpdatePublisherAsync(int id, PublisherViewModel publisherView)
        {
            try
            {
                var response = new Response<Publisher?>();
                var publisher = await _publisherRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (publisher == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundGenre;
                    return response;
                }

                var responseExist = await CheckExistAsync(publisherView, publisher);
                if (responseExist.Data == true)
                {
                    response.Errors = responseExist.Errors;
                    response.Status = responseExist.Status;
                    response.Message = responseExist.Message;

                    return response;
                }

                publisher.Name = publisherView.Name;
                await _publisherRepository.UpdateAsync(publisher);
                response.Data = publisher;
                response.Status = HttpStatusCode.NoContent;

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<Publisher?, PublisherService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool?>> DeletePublisherAsync(int id)
        {
            try
            {
                var response = new Response<bool?>()
                {
                    Status = HttpStatusCode.NoContent
                };

                var publisher = await _publisherRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (publisher == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundGenre;
                    return response;
                }

                await _publisherRepository.DeleteAsync(publisher);

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, PublisherService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool>> CheckExistAsync(PublisherViewModel publisherView, Publisher? publisherDb = null)
        {
            var response = new Response<bool>()
            {
                Data = false,
                Errors = new Dictionary<string, string[]>()
            };

            bool isExistByName;
            if (publisherDb == null)
            {
                isExistByName = await IsExistByNameAsync(publisherView.Name);
            }
            else
            {
                isExistByName = await IsExistByNameAsync(publisherView.Name, publisherDb.Name);
            }

            if (isExistByName)
            {
                response.Status = HttpStatusCode.Conflict;
                response.Data = true;
                response.Errors.Add(publisherView.Name, new[] { MessageError.EntityNameExist });
            }

            return response;
        }
        private async Task<bool> IsExistByNameAsync(string name)
        {
            return await _publisherRepository.GetAll()
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
