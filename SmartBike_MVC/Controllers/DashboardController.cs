using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBike_MVC.Models;
using Consumer; // <- IMPORTANTE: Agregamos la referencia a tu ApiService

namespace SmartBike_MVC.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApiService _apiService;

        // INYECTAMOS EL APISERVICE AQUÍ
        public DashboardController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            var nombre = User.Identity?.Name?.Split(' ')[0] ?? "Estudiante";
            return View(new InicioViewModel { NombreUsuario = nombre });
        }

        public IActionResult Beneficios()
        {
            var model = new List<BeneficioViewModel>
            {
                new() { Icono = "fa-heart", IconoBg = "sb-icon-red", Titulo = "Salud Cardiovascular",
                        Descripcion = "Pedalear 30 minutos diarios reduce el riesgo de enfermedades cardíacas hasta en un 50% y mejora notablemente tu bienestar físico y mental." },
                new() { Icono = "fa-leaf", IconoBg = "sb-icon-green", Titulo = "Reducción de CO₂",
                        Descripcion = "Cada kilómetro en bicicleta evita aproximadamente 150 g de CO₂. Tus recorridos tienen un impacto real y medible en el medio ambiente." },
                new() { Icono = "fa-bolt", IconoBg = "sb-icon-yellow", Titulo = "Movilidad Sostenible",
                        Descripcion = "La bicicleta es el medio de transporte más eficiente del campus. Ahorra tiempo, evita el tráfico y llega siempre fresco a clases." },
                new() { Icono = "fa-dollar-sign", IconoBg = "sb-icon-blue", Titulo = "Ahorro Económico",
                        Descripcion = "Usar la bicicleta puede suponer un ahorro de hasta $50 mensuales en transporte público y combustible a lo largo del semestre." },
            };
            return View(model);
        }

        public IActionResult Preguntas()
        {
            var model = new PreguntasViewModel
            {
                PreguntasSugeridas = new List<string>
                {
                    "¿Dónde puedo estacionar mi bicicleta?",
                    "¿Cómo registrar un recorrido?",
                    "¿Qué beneficios obtengo?",
                    "¿Cómo funciona SmartBike?"
                },
                Mensajes = new List<ChatMensajeViewModel>
                {
                    new() { EsBot = true, Texto = "¡Hola! Soy SmartBot 🤖, el asistente de SmartBike UTN. ¿En qué puedo ayudarte hoy?" }
                }
            };
            return View(model);
        }

        public IActionResult DatosCuriosos()
        {
            var model = new List<DatoCuriosoViewModel>
            {
                new() { Icono = "🌿", Titulo = "Cero Emisiones", Descripcion = "Una bicicleta genera 0 g de CO₂ durante su uso. Si 500 estudiantes de la UTN pedalean 1 km diario, se evitan ¡75 kg de emisiones contaminantes al día!" },
                new() { Icono = "❤️", Titulo = "Corazón más fuerte", Descripcion = "Pedalear activa el 80 % de los músculos del cuerpo. Hacerlo durante un semestre puede mejorar tu capacidad cardiovascular hasta en un 15 %." },
                new() { Icono = "🏛️", Titulo = "Campus más verde", Descripcion = "Cada recorrido que registras en SmartBike contribuye al índice de sostenibilidad de la UTN. ¡Juntos construimos un campus modelo en Ecuador!" },
                new() { Icono = "⚡", Titulo = "Velocidad ideal", Descripcion = "La velocidad promedio en bicicleta dentro de un campus es de 15 km/h — perfecta para llegar a tiempo entre edificios sin estrés." },
                new() { Icono = "💰", Titulo = "Ahorra mientras pedaleas", Descripcion = "Un estudiante puede ahorrar hasta $600 al año usando bicicleta en lugar de transporte pagado para sus traslados diarios." },
                new() { Icono = "🌍", Titulo = "Impacto global", Descripcion = "Si todos los estudiantes de Ecuador pedalearan a la universidad, se evitarían más de 500 toneladas de CO₂ al año. ¡Pequeños cambios, gran impacto!" },
            };
            return View(model);
        }

        public IActionResult Perfil()
        {
            var nombreCompleto = User.Identity?.Name ?? "Juan Pérez";
            var partes = nombreCompleto.Split(' ');
            var model = new PerfilViewModel
            {
                Nombres = partes.Length > 0 ? partes[0] : nombreCompleto,
                Apellidos = partes.Length > 1 ? partes[1] : "",
                Iniciales = string.Concat(partes.Select(p => p[0])).ToUpper(),
                Carrera = User.FindFirst("Carrera")?.Value ?? "Ingeniería en Sistemas",
                CorreoInstitucional = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? ""
            };
            return View(model);
        }

        public IActionResult Configuracion()
        {
            return View();
        }
    }
}