using Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly Lab2Context _dbcontext;

        public CategoryController(Lab2Context dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet("")]
        [Authorize]
        public async Task<ActionResult<List<Category>>> GetAll()
        {
            try
            {
                return await _dbcontext.Categories
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
