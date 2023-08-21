using Api.Models.Request;
using Api.Models.Response;
using App.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace App.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public DateTime Dob { get; set; }

        public IActionResult OnGet()
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                return Page();
            }
            else
            {
                return RedirectToPage("/Category");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            var client = new ClientService(HttpContext);
            var model = new RegisterRequest
            {
                Email = Email,
                Password = Password,
                Dob = Dob
            };
            var res = await client.Post("http://localhost:5100/api/Account/register", model);
            if (res != null)
            {
                var loginModel = new LoginRequest
                {
                    Email = Email,
                    Password = Password
                };
                var loginRes = await client.Post<LoginResponse>("http://localhost:5100/api/Account/gettoken", loginModel);
                if (loginRes == null) return RedirectToPage("/Error");
                HttpContext.Response.Cookies.Append("AccessToken", loginRes.Token);

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(loginRes.Token);
                var claimsIdentity = new ClaimsIdentity(token.Claims, "login");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync("CookieAuthentication", claimsPrincipal);
                return RedirectToPage("/Category");
            }
            return OnGet();
        }
    }
}
