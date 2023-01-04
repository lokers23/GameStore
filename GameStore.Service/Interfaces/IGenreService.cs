using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Genre;

namespace GameStore.Service.Interfaces;

public interface IGenreService
{
    //Task<Response<Genre?>> SaveGenreAsync(Genre genre);
    Task<Response<Genre?>> CreateGenreAsync(GenreViewModel genre);
    Task<Response<Genre?>> UpdateGenreAsync(int id, GenreViewModel genre);
    Task<Response<List<Genre>?>> GetGenresAsync();
    Task<Response<Genre?>> GetGenreByIdAsync(int id);
    Task<Response<bool?>> DeleteGenreAsync(int id);
}