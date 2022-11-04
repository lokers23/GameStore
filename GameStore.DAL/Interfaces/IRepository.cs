namespace GameStore.DAL.Interfaces;

public interface IRepository<T>
{
    public Task CreateAsync(T model);
    public IQueryable<T> GetAll();
    public Task<T?> GetByIdAsync(int id);
    public Task UpdateAsync(T model);
    public Task DeleteAsync(T model);
}