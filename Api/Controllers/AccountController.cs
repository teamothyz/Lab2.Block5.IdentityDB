using Api.Data;
using Api.Extensions;
using Api.Models;
using Api.Models.Request;
using Api.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IConfiguration _configuration;
        public AccountController(SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("gettoken")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest login)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
                if (!result.Succeeded) return BadRequest("Login failed");
                var user = await _userManager.FindByEmailAsync(login.Email);
                if (user == null) return NotFound();
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login.Email)
                };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["Jwt:ExpiryInDays"]));
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"], claims, expires: expiry, signingCredentials: creds);

                return new LoginResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null) return Conflict();
                user = new User
                {
                    Email = request.Email,
                    UserName = request.Email,
                    Dob = request.Dob,
                };
                var res = await _userManager.CreateAsync(user, request.Password);
                if (!res.Succeeded) return BadRequest("Create user failed");

                res = await _userManager.AddToRoleAsync(user, "user");
                if (!res.Succeeded) return BadRequest("Assign role failed");
                return Created(string.Empty, null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            try
            {
                CustomMiddleware.BlackListTokens.Add(ApiContext.Current.Token);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("addrole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleRequest request)
        {
            try
            {
                var dbRoles = await _roleManager.Roles.ToListAsync();
                if (!dbRoles.Any(role => role.Name.ToLower() == request.Role.ToLower())) return BadRequest();

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null) return NotFound();

                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any(role => role.ToLower() == request.Role.ToLower())) return Conflict();

                var rs = await _userManager.AddToRoleAsync(user, request.Role);
                if (!rs.Succeeded) return BadRequest("Add role failed");
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("remove/{email}/{role}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddRole([FromRoute] DeleteRoleRequest request)
        {
            try
            {
                var dbRoles = await _roleManager.Roles.ToListAsync();
                if (!dbRoles.Any(role => role.Name.ToLower() == request.Role.ToLower())) return BadRequest();

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null) return NotFound();

                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Any(role => role.ToLower() == request.Role.ToLower())) return NotFound();

                var rs = await _userManager.RemoveFromRoleAsync(user, request.Role.ToLower());
                if (!rs.Succeeded) return BadRequest("Remove role failed");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
