using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBike_MVC.Models; // <- aquí están EstadisticasAdminDto, PreguntaSinResponderDto, etc. (clases espejo)
using Consumer;
using Modelos;              // <- aquí están las entidades Facultad y PreguntaFrecuente

namespace SmartBike_MVC.Controllers
{
    [Authorize(Policy = "SoloAdmin")]
    public class AdminController : Controller
    {
        private readonly ApiService _apiService;

        public AdminController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // =====================================================
        // GET: /Admin/Estadisticas?dias=7&facultadId=3
        // =====================================================
        public async Task<IActionResult> Estadisticas(int dias = 7, int? facultadId = null)
        {
            string endpoint = $"Admin/Estadisticas?dias={dias}";
            if (facultadId.HasValue) endpoint += $"&facultadId={facultadId.Value}";

            var stats = await _apiService.GetAsync<EstadisticasAdminDto>(endpoint);
            var facultades = await _apiService.GetListAsync<Facultad>("Facultades");

            string? error = null;
            if (!stats.Success) error = "Error al cargar estadísticas: " + stats.Message;
            else if (!facultades.Success) error = "Error al cargar facultades: " + facultades.Message;

            var model = new EstadisticasAdminViewModel
            {
                Dias = dias,
                FacultadId = facultadId,
                Estadisticas = stats.Data ?? new EstadisticasAdminDto(),
                Facultades = facultades.Data ?? new List<Facultad>(),
                ErrorApi = error
            };

            ViewData["Title"] = "Estadísticas CO2";
            ViewData["ActiveAdmin"] = "Estadisticas";
            return View(model);
        }

        // =====================================================
        // GET: /Admin/Chatbot
        // =====================================================
        public async Task<IActionResult> Chatbot()
        {
            var sinResponder = await _apiService.GetListAsync<PreguntaSinResponderDto>("Admin/PreguntasSinResponder");
            var frecuentes = await _apiService.GetListAsync<PreguntaFrecuente>("Admin/Preguntas");

            var model = new ChatbotAdminViewModel
            {
                SinResponder = sinResponder.Data ?? new List<PreguntaSinResponderDto>(),
                PreguntasFrecuentes = frecuentes.Data ?? new List<PreguntaFrecuente>(),
                ErrorApi = (sinResponder.Success && frecuentes.Success)
                    ? null
                    : "No se pudo conectar con la API: " + (sinResponder.Message ?? frecuentes.Message)
            };

            ViewData["Title"] = "Gestión del chatbot";
            ViewData["ActiveAdmin"] = "Chatbot";
            return View(model);
        }

        // =====================================================
        // POST: /Admin/ResolverPregunta  (check ✓ de la lista)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResolverPregunta(string mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
                return RedirectToAction(nameof(Chatbot));

            var respuesta = await _apiService.PostAsync("Admin/PreguntasSinResponder/Resolver",
                new ResolverPreguntaDto { Mensaje = mensaje });

            TempData[respuesta.Success ? "AdminOk" : "AdminError"] =
                respuesta.Success ? "Pregunta marcada como atendida." : respuesta.Message;

            return RedirectToAction(nameof(Chatbot));
        }

        // =====================================================
        // POST: /Admin/CrearPregunta  (botón "+ Nueva")
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPregunta(PreguntaFrecuenteFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["AdminError"] = "El texto de la pregunta es obligatorio.";
                return RedirectToAction(nameof(Chatbot));
            }

            var nueva = new PreguntaFrecuente
            {
                TextoPregunta = model.TextoPregunta,
                RespuestaDefault = model.RespuestaDefault,
                Orden = model.Orden,
                Activo = model.Activo
            };

            var respuesta = await _apiService.PostAsync("Admin/Preguntas", nueva);
            TempData[respuesta.Success ? "AdminOk" : "AdminError"] =
                respuesta.Success ? "Pregunta agregada a la base de conocimiento." : respuesta.Message;

            return RedirectToAction(nameof(Chatbot));
        }

        // =====================================================
        // POST: /Admin/EditarPregunta  (lápiz ✏)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPregunta(PreguntaFrecuenteFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["AdminError"] = "Datos inválidos al editar la pregunta.";
                return RedirectToAction(nameof(Chatbot));
            }

            var editada = new PreguntaFrecuente
            {
                PreguntaId = model.PreguntaId,
                TextoPregunta = model.TextoPregunta,
                RespuestaDefault = model.RespuestaDefault,
                Orden = model.Orden,
                Activo = model.Activo
            };

            var respuesta = await _apiService.PutAsync($"Admin/Preguntas/{model.PreguntaId}", editada);
            TempData[respuesta.Success ? "AdminOk" : "AdminError"] =
                respuesta.Success ? "Pregunta actualizada." : respuesta.Message;

            return RedirectToAction(nameof(Chatbot));
        }

        // =====================================================
        // POST: /Admin/EliminarPregunta  (basurero 🗑)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarPregunta(int id)
        {
            var respuesta = await _apiService.DeleteAsync($"Admin/Preguntas/{id}");
            TempData[respuesta.Success ? "AdminOk" : "AdminError"] =
                respuesta.Success ? "Pregunta eliminada." : respuesta.Message;

            return RedirectToAction(nameof(Chatbot));
        }

    }
}