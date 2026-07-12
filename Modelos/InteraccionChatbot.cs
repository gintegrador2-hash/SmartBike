using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("interacciones_chatbot")]
    public class InteraccionChatbot
    {
        [Key]
        [Column("interaccion_id")]
        public int InteraccionId { get; set; }

        [Required]
        [Column("conversacion_id")]
        public int ConversacionId { get; set; }

        [Column("pregunta_id")]
        public int? PreguntaId { get; set; }

        [Column("mensaje_usuario", TypeName = "text")]
        public string? MensajeUsuario { get; set; }

        [Column("respuesta_bot", TypeName = "text")]
        public string? RespuestaBot { get; set; }

        [Required]
        [Column("fecha_interaccion")]
        public DateTime FechaInteraccion { get; set; } = DateTime.Now;

        // Propiedades de navegación
        [ForeignKey(nameof(ConversacionId))]
        public virtual ConversacionChatbot Conversacion { get; set; } = null!;

        [ForeignKey(nameof(PreguntaId))]
        public virtual PreguntaFrecuente? PreguntaFrecuente { get; set; }
    }
}
