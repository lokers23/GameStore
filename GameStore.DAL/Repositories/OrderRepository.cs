using GameStore.DAL.Interfaces;
using GameStore.Domain.Models;

namespace GameStore.DAL.Repositories;

public class OrderRepository : IRepository<Order>
{
    private readonly GamestoredbContext _db;
    public OrderRepository(GamestoredbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(Order model)
    {
        await _db.Orders.AddAsync(model);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Order model)
    {
        _db.Orders.Remove(model);
        await _db.SaveChangesAsync();
    }

    public IQueryable<Order> GetAll()
    {
        return _db.Orders;
    }

    public async Task UpdateAsync(Order model)
    {
        _db.Orders.Update(model);
        await _db.SaveChangesAsync();
    }
}