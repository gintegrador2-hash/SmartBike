using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SmartBike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampusController : ControllerBase
    {
        private readonly SmartBikeContext _context;

        public CampusController(SmartBikeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Campus>>> GetCampus()
        {
            return await _context.Set<Campus>().ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Campus>> PostCampus(Campus campus)
        {
            _context.Set<Campus>().Add(campus);
            await _context.SaveChangesAsync();
            return Ok(campus);
        }
    }
}