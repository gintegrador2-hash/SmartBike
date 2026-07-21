using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBike_MVC.Models;
using Consumer;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Modelos;
using System.Security.Claims;

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

        public IActionResult Index()
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

            // ============================================================
            // FÓRMULA ESTÁNDAR DE HUELLA DE CARBONO (metodología DEFRA):
            //   CO2 evitado = km × (factor del auto − factor del modo usado)
            // Los factores de emisión (g CO2/km) se leen de la tabla
 
            // ============================================================
            const double FactorAutoGKm = 150; // línea base: auto a gasolina promedio

            int co2Evitado = 0;
            var cedula = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(cedula) && km > 0)
            {
                // 1. Traer el tipo de transporte REAL con sus factores desde la API
                var tipo = await ObtenerTipoTransporteAsync(transporte);

                if (tipo != null)
                {
                    // 2. Aplicar la fórmula con el factor leído de la BD
                    double factorModo = (double)tipo.FactorCo2GKm;
                    if (factorModo < FactorAutoGKm)
                    {
                        co2Evitado = (int)(km * (FactorAutoGKm - factorModo));
                    }

                    // 3. Guardar el viaje en la BD a través de la API
                    var viaje = new
                    {
                        UsuarioCedula = cedula,
                        TipoTransporteId = tipo.TipoTransporteId,
                        DistanciaKm = (decimal)km,
                        Validado = false,
                        Fecha = System.DateTime.Today,
                        FechaRegistro = System.DateTime.Now
                    };
                    await _apiService.PostAsync("RegistrosViajes", viaje);
                }
            }

            TempData[llaveCo2] = co2Evitado;
            TempData[llaveRegistro] = true;

            return RedirectToAction("Index");
        }

        // Busca el tipo de transporte COMPLETO (con sus factores de emisión)
        // según lo elegido en la vista
        private async Task<Modelos.TipoTransporte?> ObtenerTipoTransporteAsync(string transporte)
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

            // Respaldo: si el nombre no coincide, se busca por factor de emisión
            tipo ??= (transporte == "bike" || transporte == "walk")
                ? tipos.Data.FirstOrDefault(t => t.FactorCo2GKm == 0)
                : tipos.Data.FirstOrDefault(t => t.FactorCo2GKm > 0);

            return tipo;
        }

        public IActionResult Estacionamientos()
        {
            return View();
        }

        public IActionResult Beneficios()
        {
            var model = new List<BeneficioViewModel>
    {
        new() { Icono = "fa-heart", IconoBg = "sb-circle-green", Titulo = "Salud Cardiovascular", Descripcion = "Pedalear 30 minutos diarios reduce el riesgo de enfermedades cardíacas..." },
        new() { Icono = "fa-leaf", IconoBg = "sb-circle-green", Titulo = "Reducción de CO₂", Descripcion = "Cada kilómetro en bicicleta evita aproximadamente 150 g de CO₂..." },
        new() { Icono = "fa-bolt", IconoBg = "sb-circle-green", Titulo = "Movilidad Sostenible", Descripcion = "La bicicleta es el medio de transporte más eficiente del campus..." },
        new() { Icono = "fa-dollar-sign", IconoBg = "sb-circle-green", Titulo = "Ahorro Económico", Descripcion = "Usar la bicicleta puede suponer un ahorro de hasta $50 mensuales..." },
    };
            return View(model);
        }

        public async Task<IActionResult> Preguntas()
        {
            // Los chips sugeridos vienen de la BD (los administra el admin en su panel)
            var respuesta = await _apiService.GetListAsync<Modelos.PreguntaFrecuente>("Chatbot/PreguntasFrecuentes");

            List<string> sugeridas;

            if (respuesta.Success && respuesta.Data != null && respuesta.Data.Any())
            {
                sugeridas = respuesta.Data.Select(p => p.TextoPregunta).ToList();
            }
            else
            {
                // Respaldo por si la API no responde o la base está vacía
                sugeridas = new List<string>
        {
            "¿Cómo funciona SmartBike?",
            "¿Cómo registrar un recorrido?"
        };
            }

            var model = new PreguntasViewModel
            {
                PreguntasSugeridas = sugeridas,
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
        new() { Icono = "fa-person-biking", IconoBg = "sb-circle-green", Titulo = "Ciclismo y salud mental", Descripcion = "Pedalear reduce el estrés hasta un 40% en trayectos cortos." },
        new() { Icono = "fa-wind",          IconoBg = "sb-circle-green", Titulo = "CO₂ por kilómetro",       Descripcion = "Una bici emite 0 g vs 120 g de un auto a gasolina." },
        new() { Icono = "fa-heart-pulse",   IconoBg = "sb-circle-green", Titulo = "Corazón más fuerte",      Descripcion = "Pedalear activa el 80 % de los músculos del cuerpo." },
        new() { Icono = "fa-dollar-sign",   IconoBg = "sb-circle-green", Titulo = "Ahorro mensual",          Descripcion = "Pedalear puede ahorrarte $80 al mes frente al transporte privado." },
        new() { Icono = "fa-droplet",       IconoBg = "sb-circle-green", Titulo = "Hidratación activa",      Descripcion = "Los ciclistas necesitan hasta 1.5 L extra de agua al día." },
        new() { Icono = "fa-earth-americas",IconoBg = "sb-circle-green", Titulo = "Campus verde",            Descripcion = "La UTN tiene 45 hectáreas disponibles para movilidad sostenible." },
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
        // GET: /Dashboard/Perfil — ahora carga los datos REALES desde la API
        public async Task<IActionResult> Perfil()
        {
            var cedula = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Usuario? usuario = null;

            if (!string.IsNullOrEmpty(cedula))
            {
                var resp = await _apiService.GetAsync<Usuario>($"Usuarios/{cedula}");
                if (resp.Success) usuario = resp.Data;
            }

            var model = new PerfilViewModel
            {
                Nombres = usuario?.Nombres ?? "Usuario",
                Apellidos = usuario?.Apellidos ?? "",
                Iniciales = string.Concat($"{usuario?.Nombres} {usuario?.Apellidos}"
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Take(2).Select(p => p[0])).ToUpper(),
                Carrera = await ObtenerNombreCarreraAsync(usuario?.CarreraId),
                CorreoInstitucional = usuario?.CorreoInstitucional ?? ""
            };
            return View(model);
        }
        private async Task<string> ObtenerNombreCarreraAsync(int? carreraId)
        {
            if (carreraId == null) return "Sin carrera registrada";
            var resp = await _apiService.GetAsync<Modelos.Carrera>($"Carreras/{carreraId}");
            return resp.Success && resp.Data != null ? resp.Data.Nombre : "Sin carrera registrada";
        }

        // POST: /Dashboard/EditarPerfil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPerfil(EditarPerfilViewModel model)
        {
            var cedulaActual = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 1. Validar campos obligatorios
            if (string.IsNullOrEmpty(cedulaActual)
                || string.IsNullOrWhiteSpace(model.Cedula)
                || string.IsNullOrWhiteSpace(model.Nombres)
                || string.IsNullOrWhiteSpace(model.Apellidos)
                || string.IsNullOrWhiteSpace(model.CorreoInstitucional))
            {
                TempData["PerfilError"] = "Todos los campos son obligatorios.";
                return RedirectToAction(nameof(Perfil));
            }

            // 2. Validar cédula con el algoritmo ecuatoriano (módulo 10)
            if (!new CedulaEcuatorianaAttribute().IsValid(model.Cedula.Trim()))
            {
                TempData["PerfilError"] = "La cédula ingresada no es una cédula ecuatoriana válida.";
                return RedirectToAction(nameof(Perfil));
            }

            // 3. Validar correo institucional
            if (!System.Text.RegularExpressions.Regex.IsMatch(
                    model.CorreoInstitucional.Trim(),
                    @"^[A-Za-z0-9._%+-]+@utn\.edu\.ec$"))
            {
                TempData["PerfilError"] = "Debe usar su correo institucional @utn.edu.ec.";
                return RedirectToAction(nameof(Perfil));
            }

            // 4. Traer el usuario completo desde la API
            var resp = await _apiService.GetAsync<Usuario>($"Usuarios/{cedulaActual}");
            if (!resp.Success || resp.Data == null)
            {
                TempData["PerfilError"] = "No se pudo cargar tu información. Intenta de nuevo.";
                return RedirectToAction(nameof(Perfil));
            }

            // 5. Actualizar los campos editables y guardar vía API (PUT)
            var usuario = resp.Data;
            usuario.Nombres = model.Nombres.Trim();
            usuario.Apellidos = model.Apellidos.Trim();
            usuario.CorreoInstitucional = model.CorreoInstitucional.Trim();

            var putResp = await _apiService.PutAsync($"Usuarios/{cedulaActual}", usuario);
            if (!putResp.Success)
            {
                TempData["PerfilError"] = "Error al guardar: " + putResp.Message;
                return RedirectToAction(nameof(Perfil));
            }

            // 6. Si además cambió la cédula, ejecutar el cambio transaccional en la API
            var cedulaNueva = model.Cedula.Trim();
            if (cedulaNueva != cedulaActual)
            {
                var cedulaResp = await _apiService.PostAsync(
                    $"Usuarios/{cedulaActual}/CambiarCedula",
                    new { NuevaCedula = cedulaNueva });

                if (!cedulaResp.Success)
                {
                    TempData["PerfilError"] = "Datos guardados, pero la cédula no se pudo cambiar: " + cedulaResp.Message;
                    return RedirectToAction(nameof(Perfil));
                }

                usuario.Cedula = cedulaNueva;
            }

            // 7. Renovar la cookie con los datos nuevos (incluida la cédula)
            await RenovarSesionAsync(usuario);

            TempData["PerfilOk"] = "Perfil actualizado correctamente.";
            return RedirectToAction(nameof(Perfil));
        }

        // POST: /Dashboard/CambiarContrasena
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarContrasena(CambiarContrasenaViewModel model)
        {
            var cedula = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(cedula))
                return RedirectToAction(nameof(Perfil));

            if (string.IsNullOrWhiteSpace(model.ContrasenaNueva) || model.ContrasenaNueva.Length < 8)
            {
                TempData["PerfilError"] = "La nueva contraseña debe tener al menos 8 caracteres.";
                return RedirectToAction(nameof(Perfil));
            }

            if (model.ContrasenaNueva != model.ContrasenaConfirmar)
            {
                TempData["PerfilError"] = "La nueva contraseña y su confirmación no coinciden.";
                return RedirectToAction(nameof(Perfil));
            }

            // El cambio se delega a la API: allí se verifica la contraseña actual
            // contra el hash BCrypt y se genera el hash de la nueva.
            // El MVC nunca maneja hashes ni compara contraseñas.
            var respuesta = await _apiService.PostAsync(
                $"Usuarios/{cedula}/CambiarContrasena",
                new { ContrasenaActual = model.ContrasenaActual, ContrasenaNueva = model.ContrasenaNueva });

            TempData[respuesta.Success ? "PerfilOk" : "PerfilError"] =
                respuesta.Success ? "Contraseña cambiada correctamente." : respuesta.Message;

            return RedirectToAction(nameof(Perfil));
        }

        // Vuelve a firmar la cookie con los datos actualizados del usuario
        private async Task RenovarSesionAsync(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, $"{usuario.Nombres} {usuario.Apellidos}"),
                new(ClaimTypes.Email, usuario.CorreoInstitucional),
                new(ClaimTypes.NameIdentifier, usuario.Cedula),
                new("RolId", usuario.RolId.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }
    }
}