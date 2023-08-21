using Api.Data;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Pages
{
    [Authorize(Roles = "admin")]
    public class ProductModel : PageModel
    {
        public List<Product> Products { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            var client = new ClientService(HttpContext);
            var products = await client.Get<List<Product>>("http://localhost:5100/api/Product");
            if (products == null) return RedirectToPage("/Error");
            Products = products;
            return Page();
        }
    }
}
