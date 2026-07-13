using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SmartBike
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ---> SOLUCIÓN AL ERROR DE FECHAS DE POSTGRESQL <---
            // Hack para timestamps de Postgres (Permite usar DateTime.Now sin convertir a UTC)
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            // -----------------------------------------------------

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<SmartBikeContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("SmartBikeContext") ?? throw new InvalidOperationException("Connection string 'SmartBikeContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirTodo", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("PermitirTodo");
            app.MapControllers();

            app.Run();
        }
    }
}