using GameStore.DAL.Interfaces;
using GameStore.Domain.Models;

namespace GameStore.DAL.Repositories
{
    public class PublisherRepository : IRepository<Publisher>
    {
        private readonly GamestoredbContext _db;
        public PublisherRepository(GamestoredbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(Publisher model)
        {
            await _db.Publishers.AddAsync(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Publisher model)
        {
            _db.Publishers.Remove(model);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Publisher> GetAll()
        {
            return _db.Publishers;
        }

        public async Task UpdateAsync(Publisher model)
        {
            _db.Publishers.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
