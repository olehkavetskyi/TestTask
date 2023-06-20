using Microsoft.EntityFrameworkCore;

namespace TestTask.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> GetByIdAsync(string id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteAllAsync();
    Task<int> SaveChangesAsync();
    public IEnumerable<T> Find(Func<T, bool> predicate);
}
