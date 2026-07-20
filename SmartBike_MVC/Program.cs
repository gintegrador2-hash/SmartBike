using Microsoft.AspNetCore.Authentication.Cookies;
using Consumer;

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

            // --- CONEXIÓN MVC -> API ---
            // 1. Buscamos la variable API_URL en Render.
            // 2. Si no la encuentra (porque estás en tu compu), usa localhost.
            string urlApi = Environment.GetEnvironmentVariable("API_URL") ?? "https://localhost:7119/api/";

            builder.Services.AddHttpClient<ApiService>(client =>
            {
                Console.WriteLine($"🚀 CONECTANDO SMARTBIKE MVC A API EN: {urlApi}");
                client.BaseAddress = new Uri(urlApi);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                // Esto es solo para que no explote si el certificado no es perfecto en desarrollo
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });
            // ---------------------------

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