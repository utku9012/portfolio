using portfolio.Models.Entities;

namespace portfolio.Repositories;

public interface IRepository<T> where T : class, IEntity
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
