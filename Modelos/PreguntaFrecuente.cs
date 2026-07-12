using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("preguntas_frecuentes")]
    public class PreguntaFrecuente
    {
        [Key]
        [Column("pregunta_id")]
        public int PreguntaId { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("texto_pregunta")]
        public string TextoPregunta { get; set; } = null!;

        [Column("respuesta_default", TypeName = "text")]
        public string? RespuestaDefault { get; set; }

        [Column("orden")]
        public int Orden { get; set; } = 0;

        [Required]
        [Column("activo")]
        public bool Activo { get; set; } = true;

        // Propiedades de navegación
        public virtual ICollection<InteraccionChatbot> Interacciones { get; set; } = new List<InteraccionChatbot>();
    }
}
