using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Interfaces;
using Ecommerce.API.Data;
using Ecommerce.API.Models;
using Microsoft.EntityFrameworkCore;
using Ecommerce.API.DTOs;

namespace Ecommerce.API.Service
{
    public class PedidoService : IPedido
    {
    private readonly AppDbContext _context;
    
    public PedidoService(AppDbContext context)
    {
        _context = context;
    }

/*
    public async  Task<IEnumerable<Pedido>> GetAllAsync()
        {
            return await _context.Pedido
           .AsNoTracking()
           .Include(p => p.Detalles)
           .ThenInclude(d => d.Producto)
           .ToListAsync();
        } */
    
    public async Task<(IEnumerable<PedidoReadDto> Items, int Total)>GetPagedAsync(int page, int pageSize)
        {
            var query = _context.Pedido.AsNoTracking();

            var total = await query.CountAsync();

            var items = await query
            .OrderByDescending(p => p.FechaPedido)
            .Skip((page -1)* pageSize)
            .Take(pageSize)
            .Select(p => new PedidoReadDto
            {
                IdPedido = p.IdPedido,
                IdCliente = p.IdCliente,
                IdMetodoPago = p.IdMetodoPago,
                FechaPedido = DateOnly.FromDateTime(p.FechaPedido),
                Total = p.Total,
                Estado = p.Estado
            })
            .ToListAsync();

            return (items, total);
        }

    public async Task<Pedido?> GetByIdAsync(int id)
        {
            return await _context.Pedido
            .Include(p => p.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(p => p.IdPedido == id);
        }

    public async Task<Pedido> CreateAsync(Pedido pedido)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if(!await _context.Cliente.AnyAsync(c => c.IdCliente == pedido.IdCliente))
            throw new Exception("El cliente no existe");

            if(pedido.Detalles == null || !pedido.Detalles.Any())
            throw new Exception("El pedido debe de contener almenos un detalle");

            pedido.Estado = "Pendiente";
            pedido.Total = 0;

            foreach(var detalle in pedido.Detalles)
            {
                var producto = await _context.Producto
                .FirstOrDefaultAsync(p => p.IdProducto == detalle.IdProducto);

                if(producto == null)
                throw new Exception($"Producto {detalle.IdProducto} no existe");

                if(producto.Existencias < detalle.Cantidad)
                throw new Exception($"Stock incuficiente para {producto.Nombre}");

                detalle.PrecioUnitario = producto.Precio;
                producto.Existencias -= detalle.Cantidad;
                //pedido.Total +=(detalle.Cantidad * producto.Precio);
                pedido.Total += detalle.SubTotal;
            }
            _context.Pedido.Add(pedido);
            await _context.SaveChangesAsync();
            } catch
            {
              await transaction.RollbackAsync();
              throw;  
            }
            

            _context.Pedido.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido> UpdateAsync(Pedido pedido)
        {
            var pedidoDb = await _context.Pedido.FindAsync(pedido.IdPedido);
            
            if (pedidoDb == null) throw new Exception("Pedido no encontrado");
            if (pedidoDb.Estado is "Completado" or "Cancelado")
                throw new Exception("No se puede modificar un pedido finalizado.");

            _context.Pedido.Update(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<bool> DeleteAsync(int id)
        {
           var pedido = await _context.Pedido.FindAsync(id);
           if(pedido == null) return false;

           if(pedido.Estado != "Pendiente")
           throw new IndexOutOfRangeException("Solo se puede eliminar pedidos pendientes");

           _context.Pedido.Remove(pedido);
           await _context.SaveChangesAsync();
           return true;
        }
        
    }
}