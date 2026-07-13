using Microsoft.AspNetCore.Authentication.Cookies;
using Consumer; // <- Importante: Agregar el namespace de tu ApiService

namespace SmartBike_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/Login";
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                });

            // INICIO DE LA CONEXIÓN MVC -> API
            builder.Services.AddHttpClient<ApiService>(client =>
            {
                // Cambia este puerto por la URL y puerto reales donde se ejecuta la API de SmartBike
                string urlFija = "https://localhost:7001/api/";

                Console.WriteLine($"🚀 CONECTANDO SMARTBIKE MVC A API: {urlFija}");
                client.BaseAddress = new Uri(urlFija);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                // Permite ignorar errores de certificado SSL en desarrollo local
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });
            // FIN DE LA CONEXIÓN

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}