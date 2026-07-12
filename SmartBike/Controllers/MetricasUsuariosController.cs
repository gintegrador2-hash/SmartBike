using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SmartBike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricasUsuariosController : ControllerBase
    {
        private readonly SmartBikeContext _context;

        public MetricasUsuariosController(SmartBikeContext context)
        {
            _context = context;
        }

        // GET: api/MetricasUsuarios/1005678901
        [HttpGet("{cedula}")]
        public async Task<ActionResult<MetricaUsuario>> GetMetricasUsuario(string cedula)
        {
            var metricas = await _context.MetricaUsuario.FindAsync(cedula);

            if (metricas == null)
            {
                // Si el usuario no ha viajado aún, devolvemos una estructura vacía en lugar de un error 404
                return Ok(new MetricaUsuario
                {
                    UsuarioCedula = cedula,
                    TotalKm = 0,
                    TotalCo2EvitadoG = 0,
                    TotalCalorias = 0,
                    TotalAhorroDolares = 0
                });
            }

            return metricas;
        }
    }
}