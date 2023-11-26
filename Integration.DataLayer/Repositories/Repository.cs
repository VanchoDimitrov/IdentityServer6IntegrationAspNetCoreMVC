using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Integration.DataLayer.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;

    private DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Delete(T entity)
    {
        _dbSet?.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entity)
    {
        _dbSet.RemoveRange(entity);
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null,
        string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (includeProperties != null)
        {
            foreach (var item in includeProperties.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }
        }
        return query.ToList();
    }

    public T Get(Expression<Func<T, bool>>? predicate, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;
        query = query.Where(predicate);
        if (includeProperties != null)
        {
            foreach (var item in includeProperties.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }
        }
        return query.FirstOrDefault();
    }
}
