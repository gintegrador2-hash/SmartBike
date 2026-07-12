using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("permisos")]
    public class Permiso
    {
        [Key]
        [Column("permiso_id")]
        public int PermisoId { get; set; }

        [Required]
        [MaxLength(60)]
        [Column("codigo_permiso")]
        public string CodigoPermiso { get; set; } = null!;

        [MaxLength(200)]
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        // Propiedades de navegación
        public virtual ICollection<Rol> Roles { get; set; } = new List<Rol>();
    }
}
