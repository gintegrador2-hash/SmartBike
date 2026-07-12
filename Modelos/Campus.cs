using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("campus")]
    public class Campus
    {
        [Key]
        [Column("campus_id")]
        public int CampusId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = null!;

        [MaxLength(150)]
        [Column("direccion")]
        public string? Direccion { get; set; }

        // Propiedades de navegación
        public virtual ICollection<Estacionamiento> Estacionamientos { get; set; } = new List<Estacionamiento>();
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
