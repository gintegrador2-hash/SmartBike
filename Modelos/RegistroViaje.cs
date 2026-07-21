using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("registros_viajes")]
    public class RegistroViaje
    {
        [Key]
        [Column("registro_id")]
        public int RegistroId { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("usuario_cedula")]
        public string UsuarioCedula { get; set; } = null!;

        [Required]
        [Column("tipo_transporte_id")]
        public int TipoTransporteId { get; set; }

        [MaxLength(20)]
        [Column("estacionamiento_id")]
        public string? EstacionamientoId { get; set; }

        [MaxLength(10)]
        [Column("validado_por")]
        public string? ValidadoPor { get; set; }

        [Required]
        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Required]
        [Column("distancia_km", TypeName = "decimal(6,2)")]
        public decimal DistanciaKm { get; set; }

        [Required]
        [Column("validado")]
        public bool Validado { get; set; } = false;

        [Required]
        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // ============================================
        // Propiedades de navegación corregidas con [NotMapped]
        // ============================================

        [ForeignKey(nameof(UsuarioCedula))]
        [NotMapped]
        public virtual Usuario? Usuario { get; set; }

        [ForeignKey(nameof(TipoTransporteId))]
        [NotMapped]
        public virtual TipoTransporte? TipoTransporte { get; set; }

        [ForeignKey(nameof(EstacionamientoId))]
        [NotMapped]
        public virtual Estacionamiento? Estacionamiento { get; set; }

        [ForeignKey(nameof(ValidadoPor))]
        [NotMapped]
        public virtual Usuario? Validador { get; set; }
    }
}
