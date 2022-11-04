using GameStore.Domain.Models;

namespace GameStore.Service.Interfaces;

public interface IGenreService
{
    Task<bool> CreateGenreAsync(Genre genre);
    Task<List<Genre>> GetGenresAsync();
    Task<Genre?> GetGenreByIdAsync(int id);
    Task<bool> UpdateGenreAsync(Genre genre);
    Task<bool> DeleteGenreAsync(int id);
}