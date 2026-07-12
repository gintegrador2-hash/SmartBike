using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Table("tipos_transporte")]
    public class TipoTransporte
    {
        [Key]
        [Column("tipo_transporte_id")]
        public int TipoTransporteId { get; set; }

        [Required]
        [MaxLength(30)]
        [Column("nombre")]
        public string Nombre { get; set; } = null!;

        [Column("factor_co2_g_km", TypeName = "decimal(10,2)")]
        public decimal FactorCo2GKm { get; set; } = 0;

        [Column("factor_calorias_km", TypeName = "decimal(10,2)")]
        public decimal FactorCaloriasKm { get; set; } = 0;

        [Column("factor_ahorro_dolar_km", TypeName = "decimal(10,2)")]
        public decimal FactorAhorroDolarKm { get; set; } = 0;

        // Propiedades de navegación
        public virtual ICollection<RegistroViaje> RegistrosViajes { get; set; } = new List<RegistroViaje>();
    }
}
