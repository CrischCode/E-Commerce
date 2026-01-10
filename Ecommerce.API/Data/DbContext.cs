using Microsoft.EntityFrameworkCore;
using Ecommerce.API.Models;

namespace Ecommerce.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Persona> Persona => Set<Persona>();
    public DbSet<Producto> Producto => Set<Producto>();
    public DbSet<Cliente> Cliente => Set<Cliente>();
    public DbSet<Empleado> Empleado => Set<Empleado>();
    public DbSet<Salario> Salario => Set<Salario>();
    public DbSet<Rol> Rol => Set<Rol>();
    public DbSet<PersonaRol> PersonaRol => Set<PersonaRol>();
    public DbSet<Categoria> Categoria => Set<Categoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Forzar que todo use el esquema 'public' (Evita errores en Supabase/Postgres)
        modelBuilder.HasDefaultSchema("public");

        // 2. Configuración de Persona (Clave Manual por el UUID de Supabase)
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.ToTable("persona");
            entity.HasKey(p => p.IdPersona);
            // IMPORTANTE: Como el UUID vendrá de Supabase, desactivamos la generación automática
            entity.Property(p => p.IdPersona).ValueGeneratedNever();
        });

        // 3. Configuración de PersonaRol (Clave Compuesta)
        modelBuilder.Entity<PersonaRol>()
            .HasKey(pr => new { pr.IdPersona, pr.IdRol });

        // 4. Mapeo de nombres
        modelBuilder.Entity<Producto>().ToTable("producto");
        modelBuilder.Entity<Cliente>().ToTable("cliente");
        modelBuilder.Entity<Empleado>().ToTable("empleado");
        modelBuilder.Entity<Salario>().ToTable("salario");
        modelBuilder.Entity<Rol>().ToTable("rol");
        modelBuilder.Entity<Categoria>().ToTable("categoria");

        base.OnModelCreating(modelBuilder);
    }
}