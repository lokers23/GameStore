using GameStore.DAL.Interfaces;
using GameStore.Domain.Models;

namespace GameStore.DAL.Repositories;

public class KeyRepository : IRepository<Key>
{
    private readonly GamestoredbContext _db;
    public KeyRepository(GamestoredbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(Key model)
    {
        await _db.Keys.AddAsync(model);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Key model)
    {
        _db.Keys.Remove(model);
        await _db.SaveChangesAsync();
    }

    public IQueryable<Key> GetAll()
    {
        return _db.Keys;
    }

    public async Task UpdateAsync(Key model)
    {
        _db.Keys.Update(model);
        await _db.SaveChangesAsync();
    }
}