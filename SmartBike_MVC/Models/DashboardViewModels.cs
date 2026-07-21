using System.Collections.Generic;

namespace SmartBike_MVC.Models
{
    public class InicioViewModel
    {
        public string NombreUsuario { get; set; } = null!;
    }

    public class BeneficioViewModel
    {
        public string Icono { get; set; } = null!;
        public string IconoBg { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
    }

    public class DatoCuriosoViewModel
    {
        public string Icono { get; set; } = null!;
        public string IconoBg { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
    }

    public class ChatMensajeViewModel
    {
        public bool EsBot { get; set; }
        public string Texto { get; set; } = null!;
    }

    public class PreguntasViewModel
    {
        public List<string> PreguntasSugeridas { get; set; } = new();
        public List<ChatMensajeViewModel> Mensajes { get; set; } = new();
    }

    public class PerfilViewModel
    {
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Iniciales { get; set; } = null!;
        public string Carrera { get; set; } = null!;
        public string CorreoInstitucional { get; set; } = null!;
    }
    public class EditarPerfilViewModel
    {
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string CorreoInstitucional { get; set; } = null!;
    }

    public class CambiarContrasenaViewModel
    {
        public string ContrasenaActual { get; set; } = null!;
        public string ContrasenaNueva { get; set; } = null!;
        public string ContrasenaConfirmar { get; set; } = null!;
    }
}