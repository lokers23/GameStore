using GameStore.Domain.Dto.Genre;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Genre;

namespace GameStore.Service.Interfaces;

public interface IGenreService
{
    Task<Response<List<GenreDto>?>> GetGenresAsync();
    Task<Response<GenreDto?>> GetGenreByIdAsync(int id);
    Task<Response<GenreDto?>> CreateGenreAsync(GenreViewModel genre);
    Task<Response<GenreDto?>> UpdateGenreAsync(int id, GenreViewModel genre);
    Task<Response<bool?>> DeleteGenreAsync(int id);
    Task<Response<bool>> CheckExistAsync(GenreViewModel genreView, int id);
}