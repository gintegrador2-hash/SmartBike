using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("roles")]
    public class Rol
    {
        [Key]
        [Column("rol_id")]
        public int RolId { get; set; }

        [Required]
        [MaxLength(60)]
        [Column("nombre_rol")]
        public string NombreRol { get; set; } = null!;

        [MaxLength(200)]
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        // Propiedades de navegación
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
    }
}
