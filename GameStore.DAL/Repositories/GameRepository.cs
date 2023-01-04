using GameStore.DAL.Interfaces;
using GameStore.Domain.Models;

namespace GameStore.DAL.Repositories
{
    public class GameRepository : IRepository<Game>
    {
        private readonly GamestoredbContext _db;
        public GameRepository(GamestoredbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(Game model)
        {
            await _db.Games.AddAsync(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Game model)
        {
            _db.Games.Remove(model);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Game> GetAll()
        {
            return _db.Games;
        }

        public async Task UpdateAsync(Game model)
        {
            _db.Games.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
