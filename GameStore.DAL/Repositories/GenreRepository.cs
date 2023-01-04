using GameStore.DAL.Interfaces;
using GameStore.Domain.Models;

namespace GameStore.DAL.Repositories
{
    public class GenreRepository : IRepository<Genre>
    {
        private readonly GamestoredbContext _db;
        public GenreRepository(GamestoredbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(Genre model)
        {
            await _db.Genres.AddAsync(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Genre model)
        {
            _db.Genres.Remove(model);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Genre> GetAll()
        {
            return _db.Genres;
        }

        //public async Task<Genre?> GetByIdAsync(int id)
        //{
        //    var genre = await _db.Genres.FirstOrDefaultAsync(genre => genre.Id == id);
        //    return genre;
        //}

        public async Task UpdateAsync(Genre model)
        {
            _db.Genres.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
