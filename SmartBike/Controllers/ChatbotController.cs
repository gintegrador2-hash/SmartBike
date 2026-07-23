using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SmartBike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {

        private readonly SmartBikeContext _context;

        public ChatbotController(SmartBikeContext context)
        {
            _context = context;
        }

        // GET: api/Chatbot/PreguntasFrecuentes
        [HttpGet("PreguntasFrecuentes")]
        public async Task<ActionResult<IEnumerable<PreguntaFrecuente>>> GetPreguntasFrecuentes()
        {
            return await _context.Set<PreguntaFrecuente>()
                                 .Where(p => p.Activo)
                                 .OrderBy(p => p.Orden)
                                 .ToListAsync();
        }

        // POST: api/Chatbot/RegistrarConversacion
        [HttpPost("RegistrarConversacion")]
        public async Task<ActionResult<ConversacionChatbot>> PostConversacion(ConversacionChatbot conversacion)
        {
            conversacion.FechaInicio = DateTime.Now;
            _context.Set<ConversacionChatbot>().Add(conversacion);
            await _context.SaveChangesAsync();
            return Ok(conversacion);
        }

        // POST: api/Chatbot/RegistrarInteraccion
        [HttpPost("RegistrarInteraccion")]
        public async Task<IActionResult> PostInteraccion(InteraccionChatbot interaccion)
        {
            interaccion.FechaInteraccion = DateTime.Now;
            _context.Set<InteraccionChatbot>().Add(interaccion);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Interacción del asistente guardada." });
        }
    }

}