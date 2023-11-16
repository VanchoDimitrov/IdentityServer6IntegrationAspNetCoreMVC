using Integration.DataLayer.Repositories;
using Integration.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.DataLayer.Repositories.ProductRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
