using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("facultades")]
    public class Facultad
    {
        [Key]
        [Column("facultad_id")]
        public int FacultadId { get; set; }

        [Required]
        [MaxLength(120)]
        [Column("nombre")]
        public string Nombre { get; set; } = null!;

        // Propiedades de navegación (Relaciones)
        public virtual ICollection<Carrera> Carreras { get; set; } = new List<Carrera>();
    }
}
