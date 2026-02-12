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
    public DbSet<DetallePedido> DetallePedido => Set<DetallePedido>();
    public DbSet<Pedido> Pedido => Set<Pedido>();
    public DbSet<MetodoPago> MetodoPago => Set<MetodoPago>();
    public DbSet<MovimientoInventario> MovimientoInventario => Set<MovimientoInventario>();
    public DbSet<Carrito> Carrito => Set<Carrito>();
    public DbSet<DetalleCarrito> DetalleCarrito => Set<DetalleCarrito>();

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

        //Pedido
         modelBuilder.Entity<Pedido>()
        .HasMany(p => p.Detalles)
        .WithOne(d => d.Pedido)
        .HasForeignKey(d => d.IdPedido)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Pedido>(entity =>
    {
        entity.ToTable("pedido");
        entity.HasKey(p => p.IdPedido);

        //Relación con Cliente
        entity.HasOne(p => p.Cliente)
            .WithMany() 
            .HasForeignKey(p => p.IdCliente);

        //Relación con MetodoPago
        entity.HasOne(p => p.MetodoPago)
            .WithMany()
            .HasForeignKey(p => p.IdMetodoPago);

        //Relación con Detalles 
        entity.HasMany(p => p.Detalles)
            .WithOne(d => d.Pedido)
            .HasForeignKey(d => d.IdPedido)
            .OnDelete(DeleteBehavior.Cascade);
    });

        //DetallePedido
        modelBuilder.Entity<DetallePedido>()
        .HasOne(d => d.Producto)
        .WithMany()
        .HasForeignKey(d => d.IdProducto);

        //Cliente
        modelBuilder.Entity<Cliente>(entity =>
        {
            //clave primaria
            entity.ToTable("cliente");
            //columnas en Postgres
            entity.Property(c => c.IdCliente).HasColumnName("id_cliente");
            entity.Property(c => c.IdPersona).HasColumnName("id_persona");
            entity.Property(c => c.FechaAlta).HasColumnName("fecha_alta");
            entity.Property(c => c.Puntos).HasColumnName("puntos");
            //Relacion a Persona
            entity.HasOne( c => c.Persona)
            .WithMany(p => p.Clientes)
            .HasForeignKey(c => c.IdPersona)
            .HasPrincipalKey(p => p.IdPersona);
        });

        // 4. Mapeo de entidades de la BD
        modelBuilder.Entity<Producto>().ToTable("producto");
        modelBuilder.Entity<Cliente>().ToTable("cliente");
        modelBuilder.Entity<Empleado>().ToTable("empleado");
        modelBuilder.Entity<Salario>().ToTable("salario");
        modelBuilder.Entity<Rol>().ToTable("rol");
        modelBuilder.Entity<Categoria>().ToTable("categoria");
        modelBuilder.Entity<DetallePedido>().ToTable("detalle_pedido");
        modelBuilder.Entity<Pedido>().ToTable("pedido");
        modelBuilder.Entity<MetodoPago>().ToTable("metodo_pago");
        modelBuilder.Entity<MovimientoInventario>().ToTable("movimiento_inventario");
        modelBuilder.Entity<Carrito>().ToTable("carrito");
        modelBuilder.Entity<DetalleCarrito>().ToTable("detalle_carrito");

        base.OnModelCreating(modelBuilder);
    }
}