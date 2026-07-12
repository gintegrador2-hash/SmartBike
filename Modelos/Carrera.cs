using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("carreras")]
    public class Carrera
    {
        [Key]
        [Column("carrera_id")]
        public int CarreraId { get; set; }

        [Required]
        [Column("facultad_id")]
        public int FacultadId { get; set; }

        [Required]
        [MaxLength(120)]
        [Column("nombre")]
        public string Nombre { get; set; } = null!;

        // Propiedades de navegación
        [ForeignKey(nameof(FacultadId))]
        public virtual Facultad Facultad { get; set; } = null!;
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
