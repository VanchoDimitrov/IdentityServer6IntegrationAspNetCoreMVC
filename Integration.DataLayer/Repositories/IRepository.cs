using System.Linq.Expressions;

namespace Integration.DataLayer.Repositories;

public interface IRepository<T> where T : class
{
    T Get(Expression<Func<T, bool>>? predicate, string? includeProperties = null);
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null,
        string? includeProperties = null);
    void Add(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entity);
}