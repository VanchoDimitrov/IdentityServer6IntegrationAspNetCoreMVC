using Integration.Models.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Integration.DataLayer.Repositories.CategoryRepository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ILogger<CategoryRepository> _logger;
        private ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger)
            :base(context)
        {
            _context = context;
            _logger = logger;
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

                // Check this. Might not be needed because of the Repository implementation.
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
