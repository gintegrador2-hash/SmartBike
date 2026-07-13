using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartBike_MVC.Models;

namespace SmartBike_MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var model = new HomeViewModel
        {
            Noticias = new List<NoticiaViewModel>
            {
                new() {
                    Fecha = "15 Jun 2025",
                    Titulo = "UTN lidera proyecto de investigación en energías renovables",
                    Descripcion = "Investigadores de la Facultad de Ingeniería desarrollan soluciones energéticas innovadoras para comunidades rurales de la región norte del Ecuador.",
                    ImagenUrl = "/img/noticia1.jpg"
                },
                new() {
                    Fecha = "10 Jun 2025",
                    Titulo = "Graduación de la cohorte 2024-2025 en modalidad presencial",
                    Descripcion = "Más de 800 nuevos profesionales se incorporan al mercado laboral tras completar exitosamente sus estudios en la Universidad Técnica del Norte.",
                    ImagenUrl = "/img/noticia2.jpg"
                },
                new() {
                    Fecha = "5 Jun 2025",
                    Titulo = "Convenio internacional con universidad española para intercambio",
                    Descripcion = "La UTN fortalece sus lazos internacionales con el nuevo acuerdo que beneficiará a estudiantes de grado y programas de posgrado.",
                    ImagenUrl = "/img/noticia3.jpg"
                }
            },
            Eventos = new List<EventoViewModel>
            {
                new() { Dia = "20", Mes = "JUN", Titulo = "Semana de la Ciencia y Tecnología 2025", Descripcion = "Exposiciones, conferencias y talleres abiertos a la comunidad universitaria y público en general." },
                new() { Dia = "25", Mes = "JUN", Titulo = "Feria de Empleo UTN 2025", Descripcion = "Más de 50 empresas nacionales presentarán oportunidades laborales para egresados y estudiantes." },
                new() { Dia = "2",  Mes = "JUL", Titulo = "Congreso Internacional de Innovación Educativa", Descripcion = "Especialistas de todo el continente compartirán avances en educación superior de calidad." }
            }
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}