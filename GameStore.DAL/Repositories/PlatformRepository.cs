using GameStore.DAL.Interfaces;
using GameStore.Domain.Models;

namespace GameStore.DAL.Repositories
{
    public class PlatformRepository: IRepository<Platform>
    {
        private readonly GamestoredbContext _db;
        public PlatformRepository(GamestoredbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(Platform model)
        {
            await _db.Platforms.AddAsync(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Platform model)
        {
            _db.Platforms.Remove(model);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Platform> GetAll()
        {
            return _db.Platforms;
        }

        public async Task UpdateAsync(Platform model)
        {
            _db.Platforms.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
