using GameStore.DAL.Interfaces;
using GameStore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.Repositories;

public class MinSpecificationRepository : IRepository<MinimumSpecification>
{
    private readonly GamestoredbContext _db;
    public MinSpecificationRepository(GamestoredbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(MinimumSpecification model)
    {
        await _db.MinimumSpecifications.AddAsync(model);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(MinimumSpecification model)
    {
        _db.MinimumSpecifications.Remove(model);
        await _db.SaveChangesAsync();
    }

    public IQueryable<MinimumSpecification> GetAll()
    {
        return _db.MinimumSpecifications;
    }
        
    public async Task UpdateAsync(MinimumSpecification model)
    {
        _db.MinimumSpecifications.Update(model);
        await _db.SaveChangesAsync();
    }
}