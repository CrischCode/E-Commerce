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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         modelBuilder.Entity<PersonaRol>()
        .HasKey(pr => new { pr.IdPersona, pr.IdRol });

    base.OnModelCreating(modelBuilder);


    }
}
