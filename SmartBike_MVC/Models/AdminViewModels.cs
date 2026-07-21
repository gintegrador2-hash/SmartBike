using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Modelos; // <- se mantiene: aquí viven Facultad y PreguntaFrecuente (entidades reales)

namespace SmartBike_MVC.Models
{
    public class EstadisticasAdminViewModel
    {
        public int Dias { get; set; } = 7;
        public int? FacultadId { get; set; }
        public EstadisticasAdminDto Estadisticas { get; set; } = new();
        public List<Facultad> Facultades { get; set; } = new();
        public string? ErrorApi { get; set; }
    }

    public class ChatbotAdminViewModel
    {
        public List<PreguntaSinResponderDto> SinResponder { get; set; } = new();
        public List<PreguntaFrecuente> PreguntasFrecuentes { get; set; } = new();
        public string? ErrorApi { get; set; }
    }

    public class PreguntaFrecuenteFormViewModel
    {
        public int PreguntaId { get; set; }

        [Required(ErrorMessage = "La pregunta es obligatoria")]
        [MaxLength(200)]
        [Display(Name = "Pregunta")]
        public string TextoPregunta { get; set; } = null!;

        [Display(Name = "Respuesta del bot")]
        public string? RespuestaDefault { get; set; }

        [Display(Name = "Orden")]
        public int Orden { get; set; } = 0;

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
    }

    // ============================================================
    // Clases espejo de las respuestas de la API Admin.
    // Deben tener las MISMAS propiedades que los DTOs declarados
    // al final de SmartBike/Controllers/AdminController.cs.
    // El ApiService deserializa el JSON por nombre de propiedad
    // (mismo patrón que LoginViewModel ↔ LoginDto en el login).
    // ============================================================

    public class EstadisticasAdminDto
    {
        public decimal Co2EvitadoKg { get; set; }
        public decimal Co2GeneradoKg { get; set; }
        public int TotalViajes { get; set; }
        public int UsuariosActivos { get; set; }
        public List<TendenciaDiaDto> Tendencia { get; set; } = new();
    }

    public class TendenciaDiaDto
    {
        public DateTime Fecha { get; set; }
        public string Etiqueta { get; set; } = null!;
        public decimal Co2EvitadoKg { get; set; }
    }

    public class PreguntaSinResponderDto
    {
        public string Mensaje { get; set; } = null!;
        public int Repeticiones { get; set; }
        public DateTime UltimaVez { get; set; }
    }

    public class ResolverPreguntaDto
    {
        public string Mensaje { get; set; } = null!;
    }
}