using Microsoft.EntityFrameworkCore;
using TestTask.Interfaces;

namespace TestTask.Data;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly UrlShortenerDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(UrlShortenerDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }
    public async Task<T> GetByIdAsync(string id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);

        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAllAsync()
    {
        _dbSet.RemoveRange(_dbSet);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public IEnumerable<T> Find(Func<T, bool> predicate)
    {
        return _dbSet.Where(predicate).ToList();
    }
}
