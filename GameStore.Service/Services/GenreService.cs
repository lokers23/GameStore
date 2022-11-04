using GameStore.DAL.Interfaces;
using GameStore.Domain.Models;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Service.Services;

public class GenreService : IGenreService
{
    private readonly IRepository<Genre> _genreRepository;
    public GenreService(IRepository<Genre> genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<bool> CreateGenreAsync(Genre genre)
    {
        try
        {
            await _genreRepository.CreateAsync(genre);
            return true;
        }
        catch (Exception exception)
        {
            return false;
        }
    }

    public async Task<List<Genre>> GetGenresAsync()
    {
        try
        {
            var genres = await _genreRepository.GetAll().ToListAsync();
            return genres;
        }
        catch (Exception exception)
        {
            var genres = new List<Genre>();
            return genres;
        }
    }

    public async Task<Genre?> GetGenreByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id can not be less than or equal to zero.", nameof(id));
            }
            
            var genre = await _genreRepository.GetByIdAsync(id);
            return genre;
        }
        catch (Exception exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateGenreAsync(Genre genre)
    {
        try
        {
            await _genreRepository.UpdateAsync(genre);
            return true;
        }
        catch (Exception exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteGenreAsync(int id)
    {
        try
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre is null)
            {
                throw new ArgumentNullException(nameof(genre), $"There is no genre with this id equal to {id}.");
            }
            
            await _genreRepository.DeleteAsync(genre);
            return true;
        }
        catch (Exception exception)
        {
            return false;
        }
    }
}