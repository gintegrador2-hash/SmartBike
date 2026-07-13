using System.Collections.Generic;

namespace SmartBike_MVC.Models
{
    public class NoticiaViewModel
    {
        public string Fecha { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string ImagenUrl { get; set; } = null!;
    }

    public class EventoViewModel
    {
        public string Dia { get; set; } = null!;
        public string Mes { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
    }

    public class HomeViewModel
    {
        public List<NoticiaViewModel> Noticias { get; set; } = new();
        public List<EventoViewModel> Eventos { get; set; } = new();
    }
}