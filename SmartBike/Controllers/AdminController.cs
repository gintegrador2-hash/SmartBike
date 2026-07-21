using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SmartBike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly SmartBikeContext _context;

        // Línea base: gramos de CO2 que emite un auto promedio por km.
        // Es el mismo factor (150 g/km) usado en el Dashboard del MVC.
        private const decimal FactorAutoGKm = 150m;

        public AdminController(SmartBikeContext context)
        {
            _context = context;
        }

        // =====================================================
        // ESTADÍSTICAS DE CO2
        // GET: api/Admin/Estadisticas?dias=7&facultadId=3
        // =====================================================
        [HttpGet("Estadisticas")]
        public async Task<ActionResult<EstadisticasAdminDto>> GetEstadisticas(int dias = 7, int? facultadId = null)
        {
            if (dias <= 0 || dias > 90) dias = 7;
            var desde = DateTime.Today.AddDays(-(dias - 1));

            // Consulta base: viajes del rango, unidos a usuario y tipo de transporte
            var baseQuery =
                from r in _context.RegistroViaje
                join u in _context.Usuario on r.UsuarioCedula equals u.Cedula
                join t in _context.TipoTransporte on r.TipoTransporteId equals t.TipoTransporteId
                where r.Fecha >= desde
                select new
                {
                    r.Fecha,
                    r.DistanciaKm,
                    r.UsuarioCedula,
                    u.CarreraId,
                    t.FactorCo2GKm
                };

            // Filtro opcional por facultad (Usuario -> Carrera -> Facultad)
            if (facultadId.HasValue)
            {
                var carrerasDeFacultad = await _context.Carrera
                    .Where(c => c.FacultadId == facultadId.Value)
                    .Select(c => (int?)c.CarreraId)
                    .ToListAsync();

                baseQuery = baseQuery.Where(x => carrerasDeFacultad.Contains(x.CarreraId));
            }

            var registros = await baseQuery.ToListAsync();

            // CO2 evitado: transportes sostenibles (factor 0) vs. la línea base del auto
            decimal co2EvitadoG = registros
                .Where(x => x.FactorCo2GKm == 0)
                .Sum(x => x.DistanciaKm * FactorAutoGKm);

            // CO2 generado: transportes con emisiones reales (bus, auto, moto...)
            decimal co2GeneradoG = registros
                .Where(x => x.FactorCo2GKm > 0)
                .Sum(x => x.DistanciaKm * x.FactorCo2GKm);

            // Tendencia día por día para el gráfico de barras
            var cultura = new CultureInfo("es-EC");
            var tendencia = new List<TendenciaDiaDto>();

            for (int i = 0; i < dias; i++)
            {
                var fecha = desde.AddDays(i);
                var kgDelDia = registros
                    .Where(x => x.Fecha.Date == fecha.Date && x.FactorCo2GKm == 0)
                    .Sum(x => x.DistanciaKm * FactorAutoGKm) / 1000m;

                tendencia.Add(new TendenciaDiaDto
                {
                    Fecha = fecha,
                    Etiqueta = dias <= 7
                        ? cultura.TextInfo.ToTitleCase(fecha.ToString("ddd", cultura).Replace(".", ""))
                        : fecha.ToString("dd/MM"),
                    Co2EvitadoKg = Math.Round(kgDelDia, 2)
                });
            }

            var resultado = new EstadisticasAdminDto
            {
                Co2EvitadoKg = Math.Round(co2EvitadoG / 1000m, 2),
                Co2GeneradoKg = Math.Round(co2GeneradoG / 1000m, 2),
                TotalViajes = registros.Count,
                UsuariosActivos = registros.Select(x => x.UsuarioCedula).Distinct().Count(),
                Tendencia = tendencia
            };

            return Ok(resultado);
        }

        // =====================================================
        // PREGUNTAS SIN RESPONDER (agrupadas y contadas)
        // GET: api/Admin/PreguntasSinResponder
        // =====================================================
        [HttpGet("PreguntasSinResponder")]
        public async Task<ActionResult<IEnumerable<PreguntaSinResponderDto>>> GetPreguntasSinResponder()
        {
            var grupos = await _context.InteraccionChatbot
                .Where(i => i.PreguntaId == null
                         && i.MensajeUsuario != null
                         && i.RespuestaBot == null)
                .GroupBy(i => i.MensajeUsuario!)
                .Select(g => new PreguntaSinResponderDto
                {
                    Mensaje = g.Key,
                    Repeticiones = g.Count(),
                    UltimaVez = g.Max(i => i.FechaInteraccion)
                })
                .OrderByDescending(p => p.Repeticiones)
                .ToListAsync();

            return Ok(grupos);
        }

        // =====================================================
        // MARCAR PREGUNTA SIN RESPONDER COMO ATENDIDA (check ✓)
        // POST: api/Admin/PreguntasSinResponder/Resolver
        // =====================================================
        [HttpPost("PreguntasSinResponder/Resolver")]
        public async Task<IActionResult> ResolverPregunta(ResolverPreguntaDto dto)
        {
            var interacciones = await _context.InteraccionChatbot
                .Where(i => i.PreguntaId == null
                         && i.RespuestaBot == null
                         && i.MensajeUsuario == dto.Mensaje)
                .ToListAsync();

            if (!interacciones.Any())
                return NotFound(new { mensaje = "No hay interacciones pendientes con ese texto." });

            foreach (var interaccion in interacciones)
            {
                interaccion.RespuestaBot = "[Atendida por administrador]";
            }

            await _context.SaveChangesAsync();
            return Ok(new { mensaje = $"Se marcaron {interacciones.Count} interacciones como atendidas." });
        }

        // =====================================================
        // CRUD DE LA BASE DE PREGUNTAS FRECUENTES
        // =====================================================

        // GET: api/Admin/Preguntas  (todas, incluidas inactivas — vista admin)
        [HttpGet("Preguntas")]
        public async Task<ActionResult<IEnumerable<PreguntaFrecuente>>> GetPreguntas()
        {
            return await _context.PreguntaFrecuente
                .OrderBy(p => p.Orden)
                .ThenBy(p => p.PreguntaId)
                .ToListAsync();
        }

        // POST: api/Admin/Preguntas
        [HttpPost("Preguntas")]
        public async Task<ActionResult<PreguntaFrecuente>> PostPregunta(PreguntaFrecuente pregunta)
        {
            pregunta.PreguntaId = 0; // el ID lo genera la BD
            _context.PreguntaFrecuente.Add(pregunta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPreguntas), new { id = pregunta.PreguntaId }, pregunta);
        }

        // PUT: api/Admin/Preguntas/5
        [HttpPut("Preguntas/{id}")]
        public async Task<IActionResult> PutPregunta(int id, PreguntaFrecuente pregunta)
        {
            if (id != pregunta.PreguntaId)
                return BadRequest(new { mensaje = "El ID de la pregunta no coincide." });

            var existe = await _context.PreguntaFrecuente.AnyAsync(p => p.PreguntaId == id);
            if (!existe)
                return NotFound(new { mensaje = "La pregunta no existe." });

            _context.Entry(pregunta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Pregunta frecuente actualizada." });
        }

        // DELETE: api/Admin/Preguntas/5
        [HttpDelete("Preguntas/{id}")]
        public async Task<IActionResult> DeletePregunta(int id)
        {
            var pregunta = await _context.PreguntaFrecuente.FindAsync(id);
            if (pregunta == null)
                return NotFound(new { mensaje = "La pregunta no existe." });

            // Si tiene interacciones asociadas, se desactiva (borrado lógico)
            // para no romper la integridad referencial. Si no, se elimina.
            bool tieneInteracciones = await _context.InteraccionChatbot
                .AnyAsync(i => i.PreguntaId == id);

            if (tieneInteracciones)
            {
                pregunta.Activo = false;
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "La pregunta tenía historial: se desactivó (borrado lógico)." });
            }

            _context.PreguntaFrecuente.Remove(pregunta);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Pregunta eliminada de la base de conocimiento." });
        }
    } // <- llave de cierre de la clase AdminController

    // ============================================================
    // DTOs del panel admin — declarados en este mismo archivo,
    // siguiendo la convención del proyecto (igual que LoginDto
    // al final de UsuariosController.cs)
    // ============================================================

    public class EstadisticasAdminDto
    {
        public decimal Co2EvitadoKg { get; set; }
        public decimal Co2GeneradoKg { get; set; }
        public int TotalViajes { get; set; }
        public int UsuariosActivos { get; set; }
        public List<TendenciaDiaDto> Tendencia { get; set; } = new();
    }

    public class TendenciaDiaDto
    {
        public DateTime Fecha { get; set; }
        public string Etiqueta { get; set; } = null!; // "Lun", "Mar" o "dd/MM"
        public decimal Co2EvitadoKg { get; set; }
    }

    public class PreguntaSinResponderDto
    {
        public string Mensaje { get; set; } = null!;
        public int Repeticiones { get; set; }
        public DateTime UltimaVez { get; set; }
    }

    public class ResolverPreguntaDto
    {
        public string Mensaje { get; set; } = null!;
    }

}