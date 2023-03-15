using GameStore.DAL.Interfaces;
using GameStore.Domain.Models;

namespace GameStore.DAL.Repositories;

public class ImageRepository : IRepository<Image>
{
    private readonly GamestoredbContext _db;
    public ImageRepository(GamestoredbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(Image model)
    {
        await _db.Images.AddAsync(model);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Image model)
    {
        _db.Images.Remove(model);
        await _db.SaveChangesAsync();
    }

    public IQueryable<Image> GetAll()
    {
        return _db.Images;
    }

    public async Task UpdateAsync(Image model)
    {
        _db.Images.Update(model);
        await _db.SaveChangesAsync();
    }
}