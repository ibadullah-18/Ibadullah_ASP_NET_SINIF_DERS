using ASP_NET_04._MVC_Product_site.Data;
using ASP_NET_04._MVC_Product_site.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ASP_NET_04._MVC_Product_site.Controllers;

public class ProductsController : Controller
{
    private readonly ProductsContext _context;

    public ProductsController(ProductsContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> IndexAsync()
    {
        return View(await _context.Products.ToListAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(product);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product is null) return NotFound();
        return View(product);
    }
}
