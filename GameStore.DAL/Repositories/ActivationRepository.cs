using GameStore.DAL.Interfaces;
using GameStore.Domain.Models;

namespace GameStore.DAL.Repositories;

public class ActivationRepository : IRepository<Activation>
{
    private readonly GamestoredbContext _db;
    public ActivationRepository(GamestoredbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(Activation model)
    {
        await _db.Activations.AddAsync(model);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Activation model)
    {
        _db.Activations.Remove(model);
        await _db.SaveChangesAsync();
    }

    public IQueryable<Activation> GetAll()
    {
        return _db.Activations;
    }

    public async Task UpdateAsync(Activation model)
    {
        _db.Activations.Update(model);
        await _db.SaveChangesAsync();
    }
}