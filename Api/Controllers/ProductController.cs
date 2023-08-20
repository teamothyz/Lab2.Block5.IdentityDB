using Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly Lab2Context _dbcontext;

        public ProductController(Lab2Context dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet("")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            try
            {
                return await _dbcontext.Products
                    .Include(p => p.Supplier)
                    .Include(p => p.Category)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("category/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Product>>> GetByCategoryId([FromRoute] int id)
        {
            try
            {
                return await _dbcontext.Products
                    .Where(p => p.CategoryId == id)
                    .Include(p => p.Supplier)
                    .Include(p => p.Category)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
