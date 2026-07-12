using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("estacionamientos")]
    public class Estacionamiento
    {
        [Key]
        [MaxLength(20)]
        [Column("estacionamiento_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string EstacionamientoId { get; set; } = null!;

        [Required]
        [Column("campus_id")]
        public int CampusId { get; set; }

        // NUEVA LLAVE FORÁNEA
        [Required]
        [Column("estado_estacionamiento_id")]
        public int EstadoEstacionamientoId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = null!;

        [Required]
        [Column("capacidad")]
        public int Capacidad { get; set; }

        // Propiedades de navegación
        [ForeignKey(nameof(CampusId))]
        public virtual Campus Campus { get; set; } = null!;

        [ForeignKey(nameof(EstadoEstacionamientoId))]
        public virtual EstadoEstacionamiento Estado { get; set; } = null!; // Navegación al catálogo

        public virtual ICollection<RegistroViaje> RegistrosViajes { get; set; } = new List<RegistroViaje>();
    }
}
