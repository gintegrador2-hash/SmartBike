using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("metricas_usuario")]
    public class MetricaUsuario
    {
        [Key]
        [MaxLength(10)]
        [Column("usuario_cedula")]
        [ForeignKey(nameof(Usuario))] // Relación 1:1 donde la PK es FK al mismo tiempo
        public string UsuarioCedula { get; set; } = null!;

        [Column("total_km", TypeName = "decimal(10,2)")]
        public decimal TotalKm { get; set; } = 0;

        [Column("total_co2_evitado_g", TypeName = "decimal(12,2)")]
        public decimal TotalCo2EvitadoG { get; set; } = 0;

        [Column("total_calorias", TypeName = "decimal(10,2)")]
        public decimal TotalCalorias { get; set; } = 0;

        [Column("total_ahorro_dolares", TypeName = "decimal(10,2)")]
        public decimal TotalAhorroDolares { get; set; } = 0;

        [Column("dias_registrados")]
        public int DiasRegistrados { get; set; } = 0;

        [Column("racha_actual_dias")]
        public int RachaActualDias { get; set; } = 0;

        [Column("mejor_racha_dias")]
        public int MejorRachaDias { get; set; } = 0;

        [Required]
        [Column("ultima_actualizacion")]
        public DateTime UltimaActualizacion { get; set; } = DateTime.Now;

        // Propiedad de navegación 1:1
        public virtual Usuario Usuario { get; set; } = null!;
    }
}
