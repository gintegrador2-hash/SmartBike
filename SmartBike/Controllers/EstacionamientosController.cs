using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SmartBike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstacionamientosController : ControllerBase
    {
        private readonly SmartBikeContext _context;

        public EstacionamientosController(SmartBikeContext context)
        {
            _context = context;
        }

        // GET: api/Estacionamientos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estacionamiento>>> GetEstacionamientos()
        {
            return await _context.Estacionamiento.ToListAsync();
        }

        // GET: api/Estacionamientos/EST-FICA-01
        [HttpGet("{id}")]
        public async Task<ActionResult<Estacionamiento>> GetEstacionamiento(string id)
        {
            var estacionamiento = await _context.Estacionamiento.FindAsync(id);
            if (estacionamiento == null) return NotFound(new { mensaje = "Estacionamiento no encontrado." });
            return estacionamiento;
        }

        // POST: api/Estacionamientos
        [HttpPost]
        public async Task<ActionResult<Estacionamiento>> PostEstacionamiento(Estacionamiento estacionamiento)
        {
            _context.Estacionamiento.Add(estacionamiento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEstacionamiento), new { id = estacionamiento.EstacionamientoId }, estacionamiento);
        }
    }
}