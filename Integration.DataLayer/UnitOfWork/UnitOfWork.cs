using Integration.DataLayer.Repositories.CategoryRepository;
using Integration.DataLayer.Repositories.ProductRepository;
using Microsoft.Extensions.Logging;

namespace Integration.DataLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ILogger<ProductRepository> _logger;

        private readonly ApplicationDbContext _context;
        public ICategoryRepository CategoryUoW { get; private set; }
        public IProductRepository ProductUoW { get; private set; }
        public UnitOfWork(ApplicationDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            CategoryUoW = new CategoryRepository(_context, (ILogger<CategoryRepository>)_logger);
            ProductUoW = new ProductRepository(_context, _logger);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
