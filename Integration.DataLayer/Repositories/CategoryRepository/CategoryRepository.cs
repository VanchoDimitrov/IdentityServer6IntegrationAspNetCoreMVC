using Integration.Models.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Integration.DataLayer.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ILogger<CategoryRepository> _logger;
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Add(Category entity)
        {
            try
            {
                _context.Add(entity);
                _context.SaveChanges();
                _logger.LogInformation($"Category saved: {entity.CategoryID}");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning($"Db Add Exception: {ex}");
            }
        }

        public void Delete(Category entity)
        {
            if (entity.CategoryID <= 0)
            {
                _logger.LogInformation($"Invalid Category ID: {entity.CategoryID}. Cannot perform {nameof(Delete)}");
                return;
            }

            try
            {
                var category = _context.Categories.FirstOrDefault(x => x.CategoryID == entity.CategoryID);

                if (category != null)
                {
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                    _logger.LogInformation($"Category deleted: {entity.CategoryID}");
                }
                else
                {
                    _logger.LogInformation($"Category not found with ID: {entity.CategoryID}. Cannot perform {nameof(Delete)}");
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Error deleting category: {ex.Message}");
                // Handle the exception or rethrow as needed
            }
        }


        public void DeleteRange(IEnumerable<Category> entities)
        {
            _context.Categories.RemoveRange(entities);
            _context.SaveChanges();
            _logger.LogInformation($"Categories deleted.");
        }

        public Category Get(Expression<Func<Category, bool>>? predicate, string? includeProperties = null)
        {
            IQueryable<Category> query = _context.Categories;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property.Trim());
                }
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<Category> GetAll(Expression<Func<Category, bool>>? predicate = null, string? includeProperties = null)
        {
            _logger.LogInformation($"{nameof(GetAll)}");
            IQueryable<Category> query = _context.Categories;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            return query.ToList();
        }

        public void Update(Category category)
        {
            var existingCategory = _context.Categories.FirstOrDefault(x => x.CategoryID == category.CategoryID);

            if (existingCategory == null)
            {
                _logger.LogInformation("There is no such category!");
                return;
            }

            try
            {
                _context.Entry(existingCategory).CurrentValues.SetValues(category);
                _context.SaveChanges();
                _logger.LogInformation($"Category updated: {category.CategoryID}");
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogWarning($"DbUpdateConcurrencyException: {e}");
            }
        }
    }
}
