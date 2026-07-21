using Microsoft.Extensions.Configuration;

namespace SmartBike.Middleware
{
    public class ApiKeyAuthMiddleware
    {
        private const string HeaderName = "X-Api-Key";
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyAuthMiddleware> _logger;

        public ApiKeyAuthMiddleware(RequestDelegate next, ILogger<ApiKeyAuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration config)
        {
            // Solo protege las rutas del chatbot; el resto de la API sigue igual por ahora
            // Cambia la comparación para que ignore mayúsculas/minúsculas
            if (context.Request.Path.StartsWithSegments("/api/chatbot", StringComparison.OrdinalIgnoreCase))
            {
                var configuredKey = config["Chatbot:ApiKey"];

                if (string.IsNullOrEmpty(configuredKey))
                {
                    _logger.LogError("Chatbot:ApiKey no está configurada en User Secrets.");
                    _logger.LogInformation("Clave configurada: {Key}", configuredKey);
             
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    return;
                }

                if (!context.Request.Headers.TryGetValue(HeaderName, out var extractedKey)
                    || extractedKey != configuredKey)
                {
                    _logger.LogInformation("Clave recibida: {Key}", extractedKey);
                    _logger.LogWarning("Intento de acceso al chatbot con API key inválida desde {Ip}",
                        context.Connection.RemoteIpAddress);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { mensaje = "API key inválida o ausente." });
                    return;
                }
            }

            await _next(context);
        }
    }
}