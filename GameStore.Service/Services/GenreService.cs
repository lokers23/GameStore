using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.Domain.Enums;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Genre;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.Genre;
using GameStore.Domain.Helpers;

namespace GameStore.Service.Services;

public class GenreService : IGenreService
{
    private readonly IRepository<Genre> _genreRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GenreService> _logger;
    public GenreService(ILogger<GenreService> logger, IRepository<Genre> genreRepository, IMapper mapper)
    {
        _logger = logger;
        _genreRepository = genreRepository;
        _mapper = mapper;
    }
    public async Task<Response<List<GenreDto>?>> GetGenresAsync(int? page, int? pageSize, string? name)
    {
        try
        {
            var response = new Response<List<GenreDto>?>();
            var genres =  _genreRepository.GetAll()
                .Where(genre => 
                    (string.IsNullOrEmpty(name) || genre.Name.StartsWith(name)));
            
            
            if (page.HasValue && pageSize.HasValue)
            {
                var totalDevelopers = await genres.CountAsync();
                var hasNextPage = totalDevelopers > page * pageSize;
                var hasPreviousPage = page > 1;
                
                genres =  genres
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
                    
                response.HasPreviousPage = hasPreviousPage;
                response.HasNextPage = hasNextPage;
            }

            response.Data = await genres.Select(genre => _mapper.Map<GenreDto>(genre))
                .ToListAsync();
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<GenreDto>?, GenreService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<GenreDto?>> GetGenreByIdAsync(int id)
    {
        try
        {
            var response = new Response<GenreDto?>();
            var genre = await _genreRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            response.Data = _mapper.Map<GenreDto>(genre);
            response.Status = HttpStatusCode.Ok;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<GenreDto?, GenreService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<GenreDto?>> CreateGenreAsync(GenreViewModel genreView)
    {
        try
        {
            var response = new Response<GenreDto?>();
            var responseExist = await CheckExistAsync(genreView);
            if (responseExist.Data)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;

                return response;
            }

            var genre = _mapper.Map<Genre>(genreView);
            await _genreRepository.CreateAsync(genre);
            
            response.Status = HttpStatusCode.Created;
            response.Message = MessageResponse.SuccessCreatedGenre;
            response.Data = _mapper.Map<GenreDto>(genre);
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<GenreDto?, GenreService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<GenreDto?>> UpdateGenreAsync(int id, GenreViewModel genreView)
    {
        try
        {
            var response = new Response<GenreDto?>();
            var genre = await _genreRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (genre == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            var responseExist = await CheckExistAsync(genreView, id);
            if (responseExist.Data)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;
                return response;
            }

            genre.Name = genreView.Name;
            await _genreRepository.UpdateAsync(genre);
            
            response.Data = _mapper.Map<GenreDto>(genre);
            response.Status = HttpStatusCode.NoContent;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<GenreDto?, GenreService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<bool?>> DeleteGenreAsync(int id)
    {
        try
        {
            var response = new Response<bool?>();
            var genre = await _genreRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundEntity;
                return response;
            }

            await _genreRepository.DeleteAsync(genre);

            response.Status = HttpStatusCode.NoContent;
            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, GenreService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<bool>> CheckExistAsync(GenreViewModel genreView, int id = 0)
    {
        var response = new Response<bool>()
        {
            Data = false,
            Errors = new Dictionary<string, string[]>()
        };

        var isExist = await _genreRepository.GetAll().AnyAsync(m => 
            m.Id != id &&
            m.Name.Equals(genreView.Name));
        
        if (isExist)
        {
            response.Status = HttpStatusCode.Conflict;
            response.Data = true;
            response.Errors.Add(nameof(Genre), new[] { MessageError.EntityExists });
        }

        return response;
    }
}