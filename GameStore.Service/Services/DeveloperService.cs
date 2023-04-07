﻿using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Publisher;
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
        private readonly IMapper _mapper;
        private readonly ILogger<DeveloperService> _logger;
        public DeveloperService(ILogger<DeveloperService> logger, IRepository<Developer> genreRepository,
            IMapper mapper)
        {
            _logger = logger;
            _developerRepository = genreRepository;
            _mapper = mapper;
        }
        public async Task<Response<List<DeveloperDto>?>> GetDevelopersAsync()
        {
            try
            {
                var response = new Response<List<DeveloperDto>?>();
                var developers = await _developerRepository.GetAll()
                    .Select(developer => _mapper.Map<DeveloperDto>(developer))
                    .ToListAsync();
                
                response.Data = developers;
                response.Status = HttpStatusCode.Ok;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<DeveloperDto>?, DeveloperService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<DeveloperDto?>> GetDeveloperByIdAsync(int id)
        {
            try
            {
                var response = new Response<DeveloperDto?>();
                var developer = await _developerRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (developer == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundEntity;
                    return response;
                }

                response.Data = _mapper.Map<DeveloperDto>(developer);
                response.Status = HttpStatusCode.Ok;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<DeveloperDto?, DeveloperService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<DeveloperDto?>> CreateDeveloperAsync(DeveloperViewModel developerView)
        {
            try
            {
                var response = new Response<DeveloperDto?>();
                var responseExist = await CheckExistAsync(developerView);
                if (responseExist.Data)
                {
                    response.Errors = responseExist.Errors;
                    response.Status = responseExist.Status;
                    response.Message = responseExist.Message;

                    return response;
                }

                var developer = _mapper.Map<Developer>(developerView);
                await _developerRepository.CreateAsync(developer);
                
                response.Status = HttpStatusCode.Created;
                response.Data = _mapper.Map<DeveloperDto>(developer);
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<DeveloperDto?, DeveloperService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<DeveloperDto?>> UpdateDeveloperAsync(int id, DeveloperViewModel developerView)
        {
            try
            {
                var response = new Response<DeveloperDto?>();
                var developer = await _developerRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (developer == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundEntity;
                    return response;
                }

                var responseExist = await CheckExistAsync(developerView, id);
                if (responseExist.Data)
                {
                    response.Errors = responseExist.Errors;
                    response.Status = responseExist.Status;
                    response.Message = responseExist.Message;
                    return response;
                }

                developer.Name = developerView.Name;
                await _developerRepository.UpdateAsync(developer);
                
                response.Data = _mapper.Map<DeveloperDto>(developer);
                response.Status = HttpStatusCode.NoContent;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<DeveloperDto?, DeveloperService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool?>> DeleteDeveloperAsync(int id)
        {
            try
            {
                var response = new Response<bool?>();
                var developer = await _developerRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (developer == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundGenre;
                    return response;
                }

                await _developerRepository.DeleteAsync(developer);

                response.Status = HttpStatusCode.NoContent;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, DeveloperService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool>> CheckExistAsync(DeveloperViewModel developerView, int id = 0)
        {
            var response = new Response<bool>()
            {
                Data = false,
                Errors = new Dictionary<string, string[]>()
            };

            var isExist = await _developerRepository.GetAll().AnyAsync(m => 
                m.Id != id &&
                m.Name.Equals(developerView.Name));
        
            if (isExist)
            {
                response.Status = HttpStatusCode.Conflict;
                response.Data = true;
                response.Errors.Add(nameof(Developer), new[] { MessageError.EntityExists });
            }

            return response;
        }
    }
}
