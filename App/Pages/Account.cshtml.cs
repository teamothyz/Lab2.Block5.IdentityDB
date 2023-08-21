using Api.Models.Request;
using Api.Models.Response;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Pages
{
    [Authorize(Roles = "admin")]
    public class AccountModel : PageModel
    {
        public List<UserResponse> Users { get; set; } = new();

        public List<string> BaseRoles { get; set; } = new() { "admin", "user", "manager" };

        [BindProperty]
        public string Email { get; set; } = null!;

        [BindProperty]
        public List<string> Roles { get; set; } = new();

        [BindProperty]
        public List<string> CurrentRoles { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            var client = new ClientService(HttpContext);
            var users = await client.Get<List<UserResponse>>("http://localhost:5100/api/Account/list");
            if (users == null) return RedirectToPage("/Error");
            Users = users;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var removedRoles = CurrentRoles.Except(Roles).ToList();
            var addedRoles = Roles.Except(CurrentRoles).ToList();
            var client = new ClientService(HttpContext);
            foreach (var role in removedRoles)
            {
                var res = await client.Delete($"http://localhost:5100/api/Account/remove/{Email}/{role}");
                if (res == null) return RedirectToPage("/Error");
            }
            foreach (var role in addedRoles)
            {
                var model = new AddRoleRequest
                {
                    Email = Email,
                    Role = role
                };
                var res = await client.Post($"http://localhost:5100/api/Account/addrole", model);
                if (res == null) return RedirectToPage("/Error");
            }
            return await OnGet();
        }
    }
}
