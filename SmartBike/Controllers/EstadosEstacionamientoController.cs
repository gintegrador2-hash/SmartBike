using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SmartBike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosEstacionamientoController : ControllerBase
    {
        private readonly SmartBikeContext _context;

        public EstadosEstacionamientoController(SmartBikeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoEstacionamiento>>> GetEstados()
        {
            return await _context.EstadoEstacionamiento.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<EstadoEstacionamiento>> PostEstado(EstadoEstacionamiento estado)
        {
            _context.EstadoEstacionamiento.Add(estado);
            await _context.SaveChangesAsync();
            return Ok(estado);
        }
    }
}