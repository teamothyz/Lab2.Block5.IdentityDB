using Api.Models.Request;
using Api.Models.Response;
using App.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace App.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;

        [DataType(DataType.EmailAddress)]
        [BindProperty]
        public string Email { get; set; } = null!;

        [BindProperty]
        public string Password { get; set; } = null!;

        public LoginModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            if (User?.Identity?.IsAuthenticated == true
                && (!HttpContext.Request.Cookies.ContainsKey("AccessToken")
                || string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["AccessToken"])))
            {
                await HttpContext.SignOutAsync();
                return Page();
            }
            else if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToPage("/Category");
            }
            else
            {
                return Page();
            }
        }

        public async Task<IActionResult> OnPost()
        {
            var client = new ClientService(HttpContext);
            var model = new LoginRequest
            {
                Email = Email,
                Password = Password
            };
            var res = await client.Post<LoginResponse>("http://localhost:5100/api/Account/gettoken", model);
            if (res == null) return RedirectToPage("/Error");
            HttpContext.Response.Cookies.Append("AccessToken", res.Token);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(res.Token);
            var claimsIdentity = new ClaimsIdentity(token.Claims, "login");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("CookieAuthentication", claimsPrincipal);
            return RedirectToPage("/Category");
        }
    }
}