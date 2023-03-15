using GameStore.DAL.Interfaces;
using GameStore.Domain.Enums;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Genre;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GameStore.Domain.Constants;
using GameStore.Domain.Helpers;

namespace GameStore.Service.Services;

public class GenreService : IGenreService
{
    private readonly IRepository<Genre> _genreRepository;
    private readonly ILogger<GenreService> _logger;
    public GenreService(ILogger<GenreService> logger, IRepository<Genre> genreRepository)
    {
        _logger = logger;
        _genreRepository = genreRepository;
    }
    public async Task<Response<Genre?>> GetGenreByIdAsync(int id)
    {
        try
        {
            var response = new Response<Genre?>()
            {
                Status = HttpStatusCode.Ok
            };

            var genre = await _genreRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            response.Data = genre;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<Genre?, GenreService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<List<Genre>?>> GetGenresAsync()
    {
        try
        {
            var response = new Response<List<Genre>?>()
            {
                Status = HttpStatusCode.Ok
            };

            var genre = await _genreRepository.GetAll().ToListAsync();
            response.Data = genre;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<List<Genre>?, GenreService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<Genre?>> CreateGenreAsync(GenreViewModel genreView)
    {
        try
        {
            var response = new Response<Genre?>();

            var responseExist = await CheckExistAsync(genreView);
            if (responseExist.Data == true)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;

                return response;
            }

            var genre = new Genre()
            {
                Name = genreView.Name
            };

            await _genreRepository.CreateAsync(genre);
            response.Status = HttpStatusCode.Created;
            response.Message = MessageResponse.SuccessCreatedGenre;
            response.Data = genre;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<Genre?, GenreService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<Genre?>> UpdateGenreAsync(int id, GenreViewModel genreView)
    {
        try
        {
            var response = new Response<Genre?>();
            var genre = await _genreRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            var responseExist = await CheckExistAsync(genreView, genre);
            if (responseExist.Data == true)
            {
                response.Errors = responseExist.Errors;
                response.Status = responseExist.Status;
                response.Message = responseExist.Message;

                return response;
            }

            genre.Name = genreView.Name;
            await _genreRepository.UpdateAsync(genre);
            response.Data = genre;
            response.Status = HttpStatusCode.NoContent;

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<Genre?, GenreService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<bool?>> DeleteGenreAsync(int id)
    {
        try
        {
            var response = new Response<bool?>()
            {
                Status = HttpStatusCode.NoContent
            };

            var genre = await _genreRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = MessageResponse.NotFoundGenre;
                return response;
            }

            await _genreRepository.DeleteAsync(genre);

            return response;
        }
        catch (Exception exception)
        {
            var response = Catcher.CatchError<bool?, GenreService>(exception, _logger);
            return response;
        }
    }
    public async Task<Response<bool>> CheckExistAsync(GenreViewModel genreView, Genre? genreDb = null)
    {
        var response = new Response<bool>()
        {
            Data = false,
            Errors = new Dictionary<string, string[]>()
        };

        bool isExistByName;
        if (genreDb == null)
        {
            isExistByName = await IsExistByNameAsync(genreView.Name);
        }
        else
        {
            isExistByName = await IsExistByNameAsync(genreView.Name, genreDb.Name);
        }

        if (isExistByName)
        {
            response.Status = HttpStatusCode.Conflict;
            response.Data = true;
            response.Errors.Add(genreView.Name, new[] { MessageError.EntityNameExist });
        }

        return response;
    }
    private async Task<bool> IsExistByNameAsync(string name)
    {
        return await _genreRepository.GetAll()
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