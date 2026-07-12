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
        public DateTime Fecha { get; set; } // Representa solo la fecha del viaje

        [Required]
        [Column("distancia_km", TypeName = "decimal(6,2)")]
        public decimal DistanciaKm { get; set; }

        [Required]
        [Column("validado")]
        public bool Validado { get; set; } = false;

        [Required]
        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Propiedades de navegación
        [ForeignKey(nameof(UsuarioCedula))]
        public virtual Usuario Usuario { get; set; } = null!;

        [ForeignKey(nameof(TipoTransporteId))]
        public virtual TipoTransporte TipoTransporte { get; set; } = null!;

        [ForeignKey(nameof(EstacionamientoId))]
        public virtual Estacionamiento? Estacionamiento { get; set; }

        [ForeignKey(nameof(ValidadoPor))]
        public virtual Usuario? Validador { get; set; }
    }
}
