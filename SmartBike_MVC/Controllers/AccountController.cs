using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SmartBike_MVC.Models;

namespace SmartBike_MVC.Controllers
{
    public class AccountController : Controller
    {
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

            // Validación temporal mientras se conecta contra Modelos.Usuario / base de datos real
            if (!model.CorreoInstitucional.EndsWith("@utn.edu.ec", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "Debe ingresar con su correo institucional @utn.edu.ec");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, "Juan Pérez"),
                new(ClaimTypes.Email, model.CorreoInstitucional),
                new("Carrera", "Ingeniería en Sistemas")
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

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // TODO: persistir en Modelos.Usuario mediante el DbContext real
            TempData["RegistroExitoso"] = "Cuenta creada correctamente. Ya puedes iniciar sesión.";
            return RedirectToAction(nameof(Login));
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