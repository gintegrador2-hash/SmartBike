using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SmartBike_MVC.Models;
using Consumer; // <- IMPORTANTE: Referencia a tu ApiService
using Modelos;  // <- IMPORTANTE: Referencia a tus Modelos (Usuario)

namespace SmartBike_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        // INYECTAMOS EL APISERVICE
        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 1. Preparamos los datos para enviarlos a la API
            var loginData = new
            {
                Correo = model.CorreoInstitucional,
                Clave = model.Contrasena
            };

            // 2. Consultamos al método /login que creamos en el Paso 1
            var respuesta = await _apiService.PostWithResponseAsync<object, Usuario>("Usuarios/login", loginData);

            if (respuesta.Success && respuesta.Data != null)
            {
                var usuarioEncontrado = respuesta.Data;

                // 3. ¡Login exitoso! Creamos la sesión (Cookies) con los datos REALES
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, $"{usuarioEncontrado.Nombres} {usuarioEncontrado.Apellidos}"),
                    new(ClaimTypes.Email, usuarioEncontrado.CorreoInstitucional),
                    new(ClaimTypes.NameIdentifier, usuarioEncontrado.Cedula),
                    new("RolId", usuarioEncontrado.RolId.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties { IsPersistent = model.Recordarme });

                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);

                return RedirectToAction("Index", "Dashboard");
            }

            // Si falla, mostramos el error
            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel();
            await CargarCarrerasAsync(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Recargar el combo antes de volver a mostrar el formulario
                await CargarCarrerasAsync(model);
                return View(model);
            }

            // 1. Mapeamos el ViewModel al Modelo real de BD
            var nuevoUsuario = new Usuario
            {
                Cedula = model.Cedula,
                Nombres = model.Nombres,
                Apellidos = model.Apellidos,
                CorreoInstitucional = model.CorreoInstitucional,
                ContrasenaHash = model.Contrasena,

                RolId = 1,      // Rol estudiante (debe coincidir con tu tabla roles)
                CampusId = 1,   // Campus principal (debe coincidir con tu tabla campus)
                CarreraId = model.CarreraId,   // <- AHORA SÍ se guarda la carrera elegida

                FechaRegistro = DateTime.Now,
                Estado = true
            };

            // 2. Guardamos mediante la API usando POST
            var respuesta = await _apiService.PostAsync("Usuarios", nuevoUsuario);

            if (respuesta.Success)
            {
                TempData["RegistroExitoso"] = "Cuenta creada correctamente. Ya puedes iniciar sesión.";
                return RedirectToAction(nameof(Login));
            }

            ModelState.AddModelError(string.Empty, "Error al registrar: " + (respuesta.Message ?? "Intente de nuevo"));
            await CargarCarrerasAsync(model);
            return View(model);
        }

        // Trae las carreras desde la API para llenar el combo del registro
        private async Task CargarCarrerasAsync(RegisterViewModel model)
        {
            var respuesta = await _apiService.GetListAsync<Modelos.Carrera>("Carreras");
            model.Carreras = (respuesta.Success && respuesta.Data != null)
                ? respuesta.Data.OrderBy(c => c.Nombre).ToList()
                : new List<Modelos.Carrera>();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}