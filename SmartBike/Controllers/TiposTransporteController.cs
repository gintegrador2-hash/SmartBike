using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SmartBike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposTransporteController : ControllerBase
    {
        private readonly SmartBikeContext _context;

        public TiposTransporteController(SmartBikeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoTransporte>>> GetTiposTransporte()
        {
            return await _context.Set<TipoTransporte>().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoTransporte>> GetTipoTransporte(int id)
        {
            var tipo = await _context.Set<TipoTransporte>().FindAsync(id);
            if (tipo == null) return NotFound();
            return tipo;
        }
    }
}