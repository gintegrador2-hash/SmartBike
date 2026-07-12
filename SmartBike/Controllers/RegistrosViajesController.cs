using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SmartBike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrosViajesController : ControllerBase
    {
        private readonly SmartBikeContext _context;

        public RegistrosViajesController(SmartBikeContext context)
        {
            _context = context;
        }

        // GET: api/RegistrosViajes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroViaje>>> GetRegistrosViajes()
        {
            return await _context.RegistroViaje.ToListAsync();
        }

        // GET: api/RegistrosViajes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroViaje>> GetRegistroViaje(int id)
        {
            var registroViaje = await _context.RegistroViaje.FindAsync(id);

            if (registroViaje == null)
            {
                return NotFound(new { mensaje = "El registro de viaje no existe." });
            }

            return registroViaje;
        }

        // POST: api/RegistrosViajes
        [HttpPost]
        public async Task<ActionResult<RegistroViaje>> PostRegistroViaje(RegistroViaje registroViaje)
        {
            // Forzar marcas de tiempo automáticas del servidor
            registroViaje.Fecha = DateTime.Today;
            registroViaje.FechaRegistro = DateTime.Now;

            try
            {
                _context.RegistroViaje.Add(registroViaje);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Captura si el Trigger de PostgreSQL (trg_validar_estacionamiento) lanza una excepción
                return BadRequest(new
                {
                    mensaje = "Error al registrar el viaje. Verifique las restricciones del transporte.",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });
            }

            return CreatedAtAction(nameof(GetRegistroViaje), new { id = registroViaje.RegistroId }, registroViaje);
        }

        // PUT: api/RegistrosViajes/5 (Para que un Administrador o Docente valide el viaje)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroViaje(int id, RegistroViaje registroViaje)
        {
            if (id != registroViaje.RegistroId)
            {
                return BadRequest(new { mensaje = "El ID del registro no coincide." });
            }

            _context.Entry(registroViaje).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.RegistroViaje.Any(e => e.RegistroId == id))
                {
                    return NotFound(new { mensaje = "Registro no encontrado para actualizar." });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { mensaje = "Registro de viaje actualizado con éxito." });
        }

        // DELETE: api/RegistrosViajes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistroViaje(int id)
        {
            var registroViaje = await _context.RegistroViaje.FindAsync(id);
            if (registroViaje == null)
            {
                return NotFound(new { mensaje = "Registro no encontrado." });
            }

            _context.RegistroViaje.Remove(registroViaje);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Registro de viaje eliminado." });
        }

        // GET: api/RegistrosViajes/Historial/1005678901
        // Endpoint extra para traer rápidamente todos los viajes de un alumno usando su cédula
        [HttpGet("Historial/{cedula}")]
        public async Task<ActionResult<IEnumerable<RegistroViaje>>> GetHistorialUsuario(string cedula)
        {
            var viajes = await _context.RegistroViaje
                .Where(v => v.UsuarioCedula == cedula)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

            return viajes;
        }
    }
}