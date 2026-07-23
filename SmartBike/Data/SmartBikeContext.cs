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
    public DbSet<Modelos.ConversacionChatbot> ConversacionChatbot { get; set; } = default!;
    public DbSet<Facultad> Facultad { get; set; } = default!;
    public DbSet<Permiso> Permiso { get; set; } = default!;
    public DbSet<Modelos.MetricaUsuario> MetricaUsuario { get; set; } = default!;
    public DbSet<Modelos.InteraccionChatbot> InteraccionChatbot { get; set; } = default!;
    public DbSet<Modelos.PreguntaFrecuente> PreguntaFrecuente { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapear la tabla intermedia N:M roles <-> permisos con la
        // misma convención snake_case del resto del esquema
        modelBuilder.Entity<Rol>()
            .HasMany(r => r.Permisos)
            .WithMany(p => p.Roles)
            .UsingEntity(j => j.ToTable("roles_permisos"));
    }

}
