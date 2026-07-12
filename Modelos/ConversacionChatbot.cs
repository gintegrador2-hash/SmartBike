using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("conversaciones_chatbot")]
    public class ConversacionChatbot
    {
        [Key]
        [Column("conversacion_id")]
        public int ConversacionId { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("usuario_cedula")]
        public string UsuarioCedula { get; set; } = null!;

        [Required]
        [Column("fecha_inicio")]
        public DateTime FechaInicio { get; set; } = DateTime.Now;

        [Column("fecha_fin")]
        public DateTime? FechaFin { get; set; }

        // Propiedades de navegación
        [ForeignKey(nameof(UsuarioCedula))]
        public virtual Usuario Usuario { get; set; } = null!;
        public virtual ICollection<InteraccionChatbot> Interacciones { get; set; } = new List<InteraccionChatbot>();
    }
}
