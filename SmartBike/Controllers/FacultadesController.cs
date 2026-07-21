using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SmartBike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultadesController : ControllerBase
    {
        private readonly SmartBikeContext _context;
        public FacultadesController(SmartBikeContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Facultad>>> GetFacultades()
        {
            return await _context.Facultad
                .OrderBy(f => f.Nombre)
                .Select(f => new Facultad
                {
                    FacultadId = f.FacultadId,
                    Nombre = f.Nombre
                })
                .ToListAsync();
        }

    }
}
