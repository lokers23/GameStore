using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Publisher;
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
        private readonly IMapper _mapper;
        private readonly ILogger<PublisherService> _logger;
        public PublisherService(ILogger<PublisherService> logger, IRepository<Publisher> publisherRepository, IMapper mapper)
        {
            _logger = logger;
            _publisherRepository = publisherRepository;
            _mapper = mapper;
        }
        public async Task<Response<List<PublisherDto>?>> GetPublishersAsync(int? page, int? pageSize)
        {
            try
            {
                var response = new Response<List<PublisherDto>?>();
                var publishers =  _publisherRepository.GetAll();
                
                if (page.HasValue && pageSize.HasValue)
                {
                    var totalDevelopers = await publishers.CountAsync();
                    var hasNextPage = totalDevelopers > page * pageSize;
                    var hasPreviousPage = page > 1;
                
                    publishers =  publishers
                        .Skip((page.Value - 1) * pageSize.Value)
                        .Take(pageSize.Value);
                    
                    response.HasPreviousPage = hasPreviousPage;
                    response.HasNextPage = hasNextPage;
                }
                
                response.Data = await publishers
                    .Select(publisher => _mapper.Map<PublisherDto>(publisher))
                    .ToListAsync();;
                response.Status = HttpStatusCode.Ok;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<PublisherDto>?, PublisherService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<PublisherDto?>> GetPublisherByIdAsync(int id)
        {
            try
            {
                var response = new Response<PublisherDto?>();
                var publisher = await _publisherRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (publisher == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundEntity;
                    return response;
                }

                response.Data = _mapper.Map<PublisherDto>(publisher);
                response.Status = HttpStatusCode.Ok;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<PublisherDto?, PublisherService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<PublisherDto?>> CreatePublisherAsync(PublisherViewModel publisherView)
        {
            try
            {
                var response = new Response<PublisherDto?>();
                var responseExist = await CheckExistAsync(publisherView);
                if (responseExist.Data)
                {
                    response.Errors = responseExist.Errors;
                    response.Status = responseExist.Status;
                    response.Message = responseExist.Message;

                    return response;
                }

                var publisher = _mapper.Map<Publisher>(publisherView);
                await _publisherRepository.CreateAsync(publisher);
                
                response.Status = HttpStatusCode.Created;
                response.Data = _mapper.Map<PublisherDto>(publisher);
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<PublisherDto?, PublisherService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<PublisherDto?>> UpdatePublisherAsync(int id, PublisherViewModel publisherView)
        {
            try
            {
                var response = new Response<PublisherDto?>();
                var publisher = await _publisherRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (publisher == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundEntity;
                    return response;
                }

                var responseExist = await CheckExistAsync(publisherView, id);
                if (responseExist.Data)
                {
                    response.Errors = responseExist.Errors;
                    response.Status = responseExist.Status;
                    response.Message = responseExist.Message;

                    return response;
                }

                publisher.Name = publisherView.Name;
                await _publisherRepository.UpdateAsync(publisher);
                
                response.Data = _mapper.Map<PublisherDto>(publisher);
                response.Status = HttpStatusCode.NoContent;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<PublisherDto?, PublisherService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool?>> DeletePublisherAsync(int id)
        {
            try
            {
                var response = new Response<bool?>();
                var publisher = await _publisherRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (publisher == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundEntity;
                    return response;
                }

                await _publisherRepository.DeleteAsync(publisher);

                response.Status = HttpStatusCode.NoContent;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, PublisherService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool>> CheckExistAsync(PublisherViewModel publisherView, int id = 0)
        {
            var response = new Response<bool>()
            {
                Data = false,
                Errors = new Dictionary<string, string[]>()
            };

            var isExist = await _publisherRepository.GetAll().AnyAsync(m => 
                m.Id != id &&
                m.Name.Equals(publisherView.Name));
        
            if (isExist)
            {
                response.Status = HttpStatusCode.Conflict;
                response.Data = true;
                response.Errors.Add(nameof(Publisher), new[] { MessageError.EntityExists });
            }

            return response;
        }
    }
}
