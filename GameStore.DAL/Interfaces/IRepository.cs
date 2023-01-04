namespace GameStore.DAL.Interfaces;

public interface IRepository<T>
{
    public Task CreateAsync(T model);
    public IQueryable<T> GetAll();
    public Task UpdateAsync(T model);
    public Task DeleteAsync(T model);
}