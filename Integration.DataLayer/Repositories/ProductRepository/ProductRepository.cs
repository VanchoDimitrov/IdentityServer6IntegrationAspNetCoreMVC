using Integration.Models.Categories;
using Integration.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Integration.DataLayer.Repositories.ProductRepository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger)
            :base(context)
        {
            _context = context;
            _logger = logger;
        }
       
        public void Update(Product product)
        {
            var existingProduct =
                _context.Products.FirstOrDefault(x => x.Id == product.Id);

            if (existingProduct == null)
            {
                _logger.LogInformation("There is no such product!");
                return;
            }

            try
            {
                _context.Entry(existingProduct).CurrentValues.SetValues(product);

                // Check this. Might not be needed because of the Repository implementation.
                _context.SaveChanges();
                _logger.LogInformation($"Product updated: {product.Id}");
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogWarning($"DbUpdateConcurrencyException: {e}");
            }
        }
    }
}
