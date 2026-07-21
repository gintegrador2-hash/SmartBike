using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBike_MVC.Models;
using Consumer;
using System.Threading.Tasks;
using System.Linq;

namespace SmartBike_MVC.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApiService _apiService;

        public DashboardController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var correoOIdentificador = User.Identity?.Name ?? "desconocido";
            var nombre = correoOIdentificador.Split(' ')[0];

            string llaveRegistro = $"YaRegistro_{correoOIdentificador}";
            string llaveCo2 = $"Co2Hoy_{correoOIdentificador}";

            bool yaRegistroHoy = TempData.Peek(llaveRegistro) != null;
            int co2EvitadoHoy = TempData.Peek(llaveCo2) != null ? System.Convert.ToInt32(TempData.Peek(llaveCo2)) : 0;
            int co2EvitadoAyer = 300;

            string mensajeComparacion = "";
            if (yaRegistroHoy)
            {
                if (co2EvitadoHoy > co2EvitadoAyer)
                    mensajeComparacion = $"Sigue así, superaste los {co2EvitadoAyer}g de ayer. ¡Mejoraste tu marca!";
                else if (co2EvitadoHoy == co2EvitadoAyer)
                    mensajeComparacion = $"Mantienes un excelente ritmo, igualaste los {co2EvitadoAyer}g de ayer.";
                else
                    mensajeComparacion = $"Ayer evitaste {co2EvitadoAyer}g. ¡Cada trayecto cuenta, mañana lo superas!";
            }

            ViewBag.YaRegistroHoy = yaRegistroHoy;
            ViewBag.Co2Hoy = co2EvitadoHoy;
            ViewBag.MensajeComparacion = mensajeComparacion;

            return View(new InicioViewModel { NombreUsuario = nombre });
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> RegistrarViaje(string transporte, string distancia)
        {
            var correoOIdentificador = User.Identity?.Name ?? "desconocido";
            string llaveRegistro = $"YaRegistro_{correoOIdentificador}";
            string llaveCo2 = $"Co2Hoy_{correoOIdentificador}";

            if (TempData.Peek(llaveRegistro) != null)
            {
                return RedirectToAction("Index");
            }

            double km = distancia switch
            {
                "<2" => 1.5,
                "2-5" => 3.5,
                "5-10" => 7.5,
                ">10" => 12.0,
                _ => 0
            };

            int co2Evitado = 0;
            if (transporte == "bike" || transporte == "walk")
            {
                co2Evitado = (int)(km * 150);
            }

            // ============================================================
            // NUEVO: guardar el viaje en la BD a través de la API
            // (antes solo se guardaba en TempData y el panel admin no veía nada)
            // ============================================================
            var cedula = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(cedula) && km > 0)
            {
                int? tipoId = await ObtenerTipoTransporteIdAsync(transporte);

                if (tipoId.HasValue)
                {
                    var viaje = new
                    {
                        UsuarioCedula = cedula,
                        TipoTransporteId = tipoId.Value,
                        DistanciaKm = (decimal)km,
                        Validado = false,
                        Fecha = System.DateTime.Today,
                        FechaRegistro = System.DateTime.Now
                    };

                    await _apiService.PostAsync("RegistrosViajes", viaje);
                }
            }
            // ============================================================

            TempData[llaveCo2] = co2Evitado;
            TempData[llaveRegistro] = true;

            return RedirectToAction("Index");
        }

        // Busca el ID del tipo de transporte en la BD según lo elegido en la vista
        private async Task<int?> ObtenerTipoTransporteIdAsync(string transporte)
        {
            var tipos = await _apiService.GetListAsync<Modelos.TipoTransporte>("TiposTransporte");
            if (!tipos.Success || tipos.Data == null || !tipos.Data.Any()) return null;

            var candidatos = transporte switch
            {
                "bike" => new[] { "bici" },
                "walk" => new[] { "camin", "pie" },
                "car" => new[] { "carro", "auto", "vehic" },
                _ => new[] { transporte ?? "" }
            };

            var tipo = tipos.Data.FirstOrDefault(t =>
                candidatos.Any(c => t.Nombre.ToLower().Contains(c)));

            // Respaldo: si no coincide el nombre, usa el factor de emisión
            tipo ??= (transporte == "bike" || transporte == "walk")
                ? tipos.Data.FirstOrDefault(t => t.FactorCo2GKm == 0)
                : tipos.Data.FirstOrDefault(t => t.FactorCo2GKm > 0);

            return tipo?.TipoTransporteId;
        }

        public IActionResult Estacionamientos()
        {
            return View();
        }

        public IActionResult Beneficios()
        {
            var model = new List<BeneficioViewModel>
            {
                new() { Icono = "fa-heart", IconoBg = "sb-icon-red", Titulo = "Salud Cardiovascular", Descripcion = "Pedalear 30 minutos diarios reduce el riesgo de enfermedades cardíacas..." },
                new() { Icono = "fa-leaf", IconoBg = "sb-icon-green", Titulo = "Reducción de CO₂", Descripcion = "Cada kilómetro en bicicleta evita aproximadamente 150 g de CO₂..." },
                new() { Icono = "fa-bolt", IconoBg = "sb-icon-yellow", Titulo = "Movilidad Sostenible", Descripcion = "La bicicleta es el medio de transporte más eficiente del campus..." },
                new() { Icono = "fa-dollar-sign", IconoBg = "sb-icon-blue", Titulo = "Ahorro Económico", Descripcion = "Usar la bicicleta puede suponer un ahorro de hasta $50 mensuales..." },
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
                new() { Icono = "🌿", Titulo = "Cero Emisiones", Descripcion = "Una bicicleta genera 0 g de CO₂ durante su uso..." },
                new() { Icono = "❤️", Titulo = "Corazón más fuerte", Descripcion = "Pedalear activa el 80 % de los músculos del cuerpo..." },
                new() { Icono = "🏛️", Titulo = "Campus más verde", Descripcion = "Cada recorrido que registras en SmartBike contribuye al índice de sostenibilidad..." },
                new() { Icono = "⚡", Titulo = "Velocidad ideal", Descripcion = "La velocidad promedio en bicicleta dentro de un campus es de 15 km/h..." },
                new() { Icono = "💰", Titulo = "Ahorra mientras pedaleas", Descripcion = "Un estudiante puede ahorrar hasta $600 al año usando bicicleta..." },
                new() { Icono = "🌍", Titulo = "Impacto global", Descripcion = "Si todos los estudiantes de Ecuador pedalearan a la universidad..." },
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

        // =======================================================
        // NUEVO MÉTODO PARA CONECTAR CON N8N
        // =======================================================
        [HttpPost]
        public async Task<IActionResult> EnviarMensajeN8N([FromBody] string mensajeUsuario)
        {
            if (string.IsNullOrEmpty(mensajeUsuario)) return BadRequest("Mensaje vacío");

            // URL exacta de tu webhook en n8n
            string webhookUrl = "https://anzbsx.app.n8n.cloud/webhook/smartbot-chat";

            var payload = new
            {
                mensaje = mensajeUsuario,
                usuario = User.Identity?.Name ?? "Estudiante"
            };

            using var httpClient = new System.Net.Http.HttpClient();
            var content = new System.Net.Http.StringContent(
                System.Text.Json.JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            try
            {
                // Disparamos la petición a n8n
                var response = await httpClient.PostAsync(webhookUrl, content);
                var jsonRespuesta = await response.Content.ReadAsStringAsync();

                // Devolvemos el JSON de n8n a la vista
                return Content(jsonRespuesta, "application/json");
            }
            catch (System.Exception)
            {
                return Json(new { respuesta = "Error de conexión con SmartBot. Revisa que n8n esté activo." });
            }
        }
    }
}