using ASP_NET_04._MVC_Product_Site___Scaffold.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_04._MVC_Product_Site___Scaffold.Data
{
    public class ProductsContext: DbContext
    {
        public ProductsContext(DbContextOptions options)
        : base(options)
        { }

        protected ProductsContext()
        {
        }
        public DbSet<Product> Products => Set<Product>();
    }
}
