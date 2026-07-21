using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Modelos;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;
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

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (_context.Usuario.Any(u => u.Cedula == usuario.Cedula))
            {
                return Conflict(new { mensaje = "Ya existe un usuario registrado con esa cédula." });
            }

            // Hashear la contraseña con BCrypt antes de persistirla (RSEG-03).
            // BCrypt genera un salt aleatorio por contraseña y es intencionalmente
            // lento, lo que lo hace resistente a ataques de fuerza bruta.
            usuario.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(usuario.ContrasenaHash);

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
        // POST: api/Usuarios/login
        // POST: api/Usuarios/login
        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login([FromBody] LoginDto loginData)
        {
            // 1. Buscar únicamente por correo (el hash no se puede comparar en SQL)
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.CorreoInstitucional == loginData.Correo);

            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Correo o contraseña incorrectos." });
            }

            // 2. Verificar la contraseña contra el hash almacenado
            bool esValida;
            try
            {
                esValida = BCrypt.Net.BCrypt.Verify(loginData.Clave, usuario.ContrasenaHash);
            }
            catch
            {
                // El hash almacenado no tiene formato BCrypt (usuario antiguo en texto plano)
                esValida = false;
            }

            if (!esValida)
            {
                return Unauthorized(new { mensaje = "Correo o contraseña incorrectos." });
            }

            return Ok(usuario);
        }
        // POST: api/Usuarios/1005678901/CambiarContrasena
        [HttpPost("{cedula}/CambiarContrasena")]
        public async Task<IActionResult> CambiarContrasena(string cedula, [FromBody] CambiarContrasenaDto dto)
        {
            var usuario = await _context.Usuario.FindAsync(cedula);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            if (string.IsNullOrWhiteSpace(dto.ContrasenaNueva) || dto.ContrasenaNueva.Length < 8)
                return BadRequest(new { mensaje = "La nueva contraseña debe tener al menos 8 caracteres." });

            // Verificar la contraseña actual contra el hash
            bool actualValida;
            try
            {
                actualValida = BCrypt.Net.BCrypt.Verify(dto.ContrasenaActual, usuario.ContrasenaHash);
            }
            catch
            {
                actualValida = false;
            }

            if (!actualValida)
                return BadRequest(new { mensaje = "La contraseña actual es incorrecta." });

            // Guardar la nueva contraseña hasheada
            usuario.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(dto.ContrasenaNueva);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Contraseña actualizada correctamente." });
        }
        public class CambiarContrasenaDto
        {
            public string ContrasenaActual { get; set; } = null!;
            public string ContrasenaNueva { get; set; } = null!;
        }

        // Agrega esta pequeña clase al final del mismo archivo para recibir los datos de login
        public class LoginDto
        {
            public string Correo { get; set; } = null!;
            public string Clave { get; set; } = null!;
        }
    }
}