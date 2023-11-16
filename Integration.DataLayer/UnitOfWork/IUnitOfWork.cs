using Integration.DataLayer.Repositories.CategoryRepository;
using Integration.DataLayer.Repositories.ProductRepository;

namespace Integration.DataLayer.UnitOfWork;

public interface IUnitOfWork
{
    ICategoryRepository CategoryUoW { get; }
    IProductRepository ProductUoW { get; }

    void Save();
}
