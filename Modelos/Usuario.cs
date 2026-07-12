using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [MaxLength(10)]
        [Column("cedula")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Clave Natural
        public string Cedula { get; set; } = null!;

        [Required]
        [Column("rol_id")]
        public int RolId { get; set; }

        [Required]
        [Column("campus_id")]
        public int CampusId { get; set; }

        [Column("carrera_id")]
        public int? CarreraId { get; set; }

        [Required]
        [MaxLength(80)]
        [Column("nombres")]
        public string Nombres { get; set; } = null!;

        [Required]
        [MaxLength(80)]
        [Column("apellidos")]
        public string Apellidos { get; set; } = null!;

        [Required]
        [MaxLength(120)]
        [EmailAddress]
        [Column("correo_institucional")]
        public string CorreoInstitucional { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        [Column("contrasena_hash")]
        public string ContrasenaHash { get; set; } = null!;

        [MaxLength(255)]
        [Column("avatar_url")]
        public string? AvatarUrl { get; set; }

        [Required]
        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        [Column("estado")]
        public bool Estado { get; set; } = true;

        // Propiedades de navegación
        [ForeignKey(nameof(RolId))]
        public virtual Rol Rol { get; set; } = null!;

        [ForeignKey(nameof(CampusId))]
        public virtual Campus Campus { get; set; } = null!;

        [ForeignKey(nameof(CarreraId))]
        public virtual Carrera? Carrera { get; set; }

        // Relación 1:1 inversa con Métricas
        public virtual MetricaUsuario? Metricas { get; set; }

        public virtual ICollection<RegistroViaje> ViajesRealizados { get; set; } = new List<RegistroViaje>();
        public virtual ICollection<RegistroViaje> ViajesValidados { get; set; } = new List<RegistroViaje>();
        public virtual ICollection<ConversacionChatbot> Conversaciones { get; set; } = new List<ConversacionChatbot>();
    }
}
