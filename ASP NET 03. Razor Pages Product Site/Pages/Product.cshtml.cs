using ASP_NET_03._Razor_Pages_Product_Site.Models;
using ASP_NET_03._Razor_Pages_Product_Site.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ASP_NET_03._Razor_Pages_Product_Site.Pages
{
    public class ProductModel : PageModel
    {
        public Product Product { get; set; }
        private readonly ProductService _service;

        public ProductModel(ProductService service)
        {
            _service = service;
        }
        public async Task OnGet(int id)
        {
            Product = await _service.GetProductByIdAsync(id);
        }
    }
}
