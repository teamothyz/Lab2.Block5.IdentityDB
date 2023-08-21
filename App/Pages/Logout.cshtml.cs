using App.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Pages
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            var client = new ClientService(HttpContext);
            _ = await client.Delete("http://localhost:5100/api/Account/logout");
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Login");
        }
    }
}
