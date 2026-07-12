using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("estados_estacionamiento")]
    public class EstadoEstacionamiento
    {
        [Key]
        [Column("estado_estacionamiento_id")]
        public int EstadoEstacionamientoId { get; set; }

        [Required]
        [MaxLength(40)]
        [Column("nombre")]
        public string Nombre { get; set; } = null!; // "Disponible", "Mantenimiento", "Cerrado"

        [MaxLength(150)]
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        // Propiedad de navegación inversa
        public virtual ICollection<Estacionamiento> Estacionamientos { get; set; } = new List<Estacionamiento>();
    }
}
