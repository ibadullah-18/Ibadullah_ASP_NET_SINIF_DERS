using ASP_NET_04._MVC_Product_site.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_04._MVC_Product_site.Data;

public class ProductsContext : DbContext
{
    public ProductsContext(DbContextOptions options) 
        : base(options)
    {}

    protected ProductsContext()
    {
    }
    public DbSet<Product> Products => Set<Product>();
}
