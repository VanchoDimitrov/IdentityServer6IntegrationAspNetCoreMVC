using Integration.Models.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Integration.DataLayer.Repositories.CategoryRepository;

public class CategoryRepository : ICategoryRepository
{
    private readonly Logger<CategoryRepository> _logger;

    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context, Logger<CategoryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public void Add(Category entity)
    {
        try
        {
            _context.Add(entity);
            _context.SaveChanges();
            _logger.LogInformation($"Category Saved.{nameof(Add)}");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning($"Db Add Exception: {ex}");
        }
    }

    public void Delete(Category entity)
    {
        if (entity.CategoryID == null)
        {
            _logger.LogInformation($"Category is empty {nameof(Delete)}");
        }

        var category = _context.categories
            .AsNoTracking()
            .FirstOrDefault(x => x.CategoryID == entity.CategoryID);

        if (category != null)
        {
            _context.categories.Remove(category);
            _context.SaveChanges();
            _logger.LogInformation($"Category saved: {entity.CategoryID}");
        }
    }

    public void DeleteRange(IEnumerable<Category> entity)
    {
        _context.categories.RemoveRange(entity);
        _context.SaveChanges();
        _logger.LogInformation($"Categories deleted.");
    }

    public Category Get(Expression<Func<Category, bool>>? predicate, string? includeProperties = null)
    {
        IQueryable<Category> query = _context.categories;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var property in includeProperties
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property.Trim());
            }
        }
        return query.FirstOrDefault();
    }

    public IEnumerable<Category> GetAll(Expression<Func<Category, bool>>? predicate = null, string? includeProperties = null)
    {
        _logger.LogInformation($"{nameof(GetAll)}");
        IQueryable<Category> query = _context.categories;

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

    public void Update(Category category)
    {
        var cat = _context.categories.FirstOrDefault(x => x.CategoryID == category.CategoryID);
        if (cat != null)
        {
            _logger.LogInformation("There is no such category!");
        }
        try
        {
            _context.categories.Update(cat);
            _context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogWarning($"DbUpdateConcurencyException: {e}");
        }
    }
}