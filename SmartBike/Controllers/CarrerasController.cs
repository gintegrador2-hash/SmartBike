using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SmartBike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrerasController : ControllerBase
    {
        private readonly SmartBikeContext _context;

        public CarrerasController(SmartBikeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carrera>>> GetCarreras()
        {
            return await _context.Set<Carrera>().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Carrera>> GetCarrera(int id)
        {
            var carrera = await _context.Set<Carrera>().FindAsync(id);
            if (carrera == null) return NotFound();
            return carrera;
        }

        [HttpPost]
        public async Task<ActionResult<Carrera>> PostCarrera(Carrera carrera)
        {
            _context.Set<Carrera>().Add(carrera);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCarrera), new { id = carrera.CarreraId }, carrera);
        }
    }
}