using GameStore.DAL.Interfaces;
using GameStore.Domain.Models;

namespace GameStore.DAL.Repositories
{
    public class DeveloperRepository : IRepository<Developer>
    {
        private readonly GamestoredbContext _db;
        public DeveloperRepository(GamestoredbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(Developer model)
        {
            await _db.Developers.AddAsync(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Developer model)
        {
            _db.Developers.Remove(model);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Developer> GetAll()
        {
            return _db.Developers;
        }

        public async Task UpdateAsync(Developer model)
        {
            _db.Developers.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
