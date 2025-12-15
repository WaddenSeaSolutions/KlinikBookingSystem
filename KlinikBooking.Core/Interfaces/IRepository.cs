namespace KlinikBooking.Core.Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetAsync(int id);
    Task AddAsync(T entity);
    Task EditAsync(T entity);
    Task RemoveAsync(int id);
}