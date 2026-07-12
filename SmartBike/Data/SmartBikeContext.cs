using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modelos;

    public class SmartBikeContext : DbContext
    {
        public SmartBikeContext (DbContextOptions<SmartBikeContext> options)
            : base(options)
        {
        }

        public DbSet<Modelos.Usuario> Usuario { get; set; } = default!;
        public DbSet<Modelos.RegistroViaje> RegistroViaje { get; set; } = default!;
        public DbSet<Modelos.Rol> Rol { get; set; } = default!;
    public DbSet<Modelos.Campus> Campus { get; set; } = default!;
    public DbSet<Modelos.Carrera> Carrera { get; set; } = default!;
    public DbSet<Modelos.TipoTransporte> TipoTransporte { get; set; } = default!;
    public DbSet<Modelos.Estacionamiento> Estacionamiento { get; set; } = default!;
    public DbSet<Modelos.ConversacionChatbot> ConversacionChatbot { get; set; } = default!;
    public DbSet<Modelos.EstadoEstacionamiento> EstadoEstacionamiento { get; set; } = default!;
    public DbSet<Facultad> Facultad { get; set; } = default!;
    public DbSet<Permiso> Permiso { get; set; } = default!;
    public DbSet<Modelos.MetricaUsuario> MetricaUsuario { get; set; } = default!;

    public DbSet<Modelos.InteraccionChatbot> InteraccionChatbot { get; set; } = default!;
    public DbSet<Modelos.PreguntaFrecuente> PreguntaFrecuente { get; set; } = default!;

}
