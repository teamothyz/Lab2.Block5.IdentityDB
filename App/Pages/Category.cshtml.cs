using Api.Data;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Pages
{
    [Authorize]
    public class CategoryModel : PageModel
    {
        public List<Category> Categories { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            var client = new ClientService(HttpContext);
            var categories = await client.Get<List<Category>>("http://localhost:5100/api/Category");
            if (categories == null) return RedirectToPage("/Error");
            Categories = categories;
            return Page(); 
        }
    }
}
