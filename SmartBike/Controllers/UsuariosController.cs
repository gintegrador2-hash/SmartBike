using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SmartBike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly SmartBikeContext _context;

        public UsuariosController(SmartBikeContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuario.ToListAsync();
        }

        // GET: api/Usuarios/1005678901
        [HttpGet("{cedula}")]
        public async Task<ActionResult<Usuario>> GetUsuario(string cedula)
        {
            var usuario = await _context.Usuario.FindAsync(cedula);
            if (usuario == null) return NotFound(new { mensaje = "Usuario no encontrado." });
            return usuario;
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (_context.Usuario.Any(u => u.Cedula == usuario.Cedula))
            {
                return Conflict(new { mensaje = "Ya existe un usuario registrado con esa cédula." });
            }

            usuario.FechaRegistro = DateTime.Now;
            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { cedula = usuario.Cedula }, usuario);
        }

        // PUT: api/Usuarios/1005678901
        [HttpPut("{cedula}")]
        public async Task<IActionResult> PutUsuario(string cedula, Usuario usuario)
        {
            if (cedula != usuario.Cedula) return BadRequest(new { mensaje = "La cédula no coincide." });

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Datos de usuario actualizados." });
        }

        // DELETE: api/Usuarios/1005678901
        [HttpDelete("{cedula}")]
        public async Task<IActionResult> DeleteUsuario(string cedula)
        {
            var usuario = await _context.Usuario.FindAsync(cedula);
            if (usuario == null) return NotFound();

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Usuario eliminado del sistema." });
        }
    }
}