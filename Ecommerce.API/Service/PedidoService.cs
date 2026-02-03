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

        public async Task<(IEnumerable<PedidoReadDto> Items, int Total)> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.Pedido.AsNoTracking();
            /*
            .Include(p => p.Cliente)
              .ThenInclude(c => c!.Persona)
            .Include(p => p.MetodoPago)
            ; */

            var total = await query.CountAsync();

            var items = await query
            .OrderByDescending(p => p.FechaPedido)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PedidoReadDto
            {
                IdPedido = p.IdPedido,
                IdCliente = p.IdCliente,
                Cliente = p.Cliente != null && p.Cliente.Persona != null
                ? p.Cliente.Persona.PrimerNombre + " " + p.Cliente.Persona.PrimerApellido : "Sin nombre",
                IdMetodoPago = p.IdMetodoPago,
                MetodoPago = p.MetodoPago != null ? p.MetodoPago.Nombre : "No especificadp",
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
            .AsNoTracking()
            .Include(p => p.Cliente)
              .ThenInclude(c => c!.Persona)
            .Include(p => p.MetodoPago)
            .Include(p => p.Detalles)
              .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(p => p.IdPedido == id);
        }

        public async Task<Pedido?> CreateAsync(Pedido pedido)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (!await _context.Cliente.AnyAsync(c => c.IdCliente == pedido.IdCliente))
                    throw new Exception("El cliente no existe");

                if (pedido.Detalles == null || !pedido.Detalles.Any())
                    throw new Exception("El pedido debe de contener almenos un detalle");

                pedido.Estado = "Pendiente";
                pedido.FechaPedido = DateTime.Now;
                pedido.Total = 0;

                //tenemos que validar el stock y calcular precisos
                foreach (var detalle in pedido.Detalles)
                {
                    var producto = await _context.Producto
                    .FirstOrDefaultAsync(p => p.IdProducto == detalle.IdProducto);

                    if (producto == null)
                        throw new Exception($"Producto {detalle.IdProducto} no existe");

                    if (producto.Existencias < detalle.Cantidad)
                        throw new Exception($"Stock incuficiente para {producto.Nombre}");

                    detalle.PrecioUnitario = producto.Precio; //se congela el precio al momneto de la compra en la tabla detalle pedido
                    producto.Existencias -= detalle.Cantidad; //se resta el stock en la BD
                                                              // decimal SubTotal = detalle.Cantidad * producto.Precio; //calculo por item
                    pedido.Total += (detalle.Cantidad * producto.Precio); //sumamos cada producto

                }

                //Puntos del cliente
                var cliente = await _context.Cliente.FirstOrDefaultAsync(c => c.IdCliente == pedido.IdCliente);
                if (cliente != null)
                {
                    cliente.Puntos += (int)(pedido.Total / 10); // un punto por cada 10 unidades monetarias
                }

                _context.Pedido.Add(pedido);
                await _context.SaveChangesAsync();

                foreach (var detalle in pedido.Detalles)
                {
                    //Registro de movimientos de inventario tipo Salida
                    var movimientoSalida = new MovimientoInventario
                    {
                        IdProducto = detalle.IdProducto,
                        TipoMovimiento = "Salida",
                        Cantidad = detalle.Cantidad,
                        Motivo = $"Venta - Pedido #{pedido.IdPedido}",
                        FechaMovimiento = DateTime.Now

                    };

                    _context.MovimientoInventario.Add(movimientoSalida);
                }

                //_context.Pedido.Add(pedido);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await _context.Pedido
                .AsNoTracking()
                .Include(p => p.Cliente)
                    .ThenInclude(c => c!.Persona)
                .Include(p => p.MetodoPago)
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.IdPedido == pedido.IdPedido);

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, PedidoUpdateDto dto)
        {
            var pedidoUpdate = await _context.Pedido
            .Include(p => p.Detalles)
            .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedidoUpdate == null) return false;

            //por si el pedido cambia a cancelado devolvemos el stock
            if (!string.IsNullOrWhiteSpace(dto.Estado) &&
            dto.Estado == "Cancelado" && pedidoUpdate.Estado != "Cancelado")
            {
                foreach (var detalle in pedidoUpdate.Detalles)
                {
                    var producto = await _context.Producto.FindAsync(detalle.IdProducto);
                    if (producto != null) producto.Existencias += detalle.Cantidad; //si el pedido se cancela le devolvemos los items a la BD

                    var movimientoEntrada = new MovimientoInventario
                    {
                        IdProducto = detalle.IdProducto,
                        TipoMovimiento = "Entrada",
                        Cantidad = detalle.Cantidad,
                        Motivo = $"Cancelacion de pedido#{pedidoUpdate.IdPedido}",
                        FechaMovimiento = DateTime.Now
                    };

                    _context.MovimientoInventario.Add(movimientoEntrada);

                }

            }

            //aqui actualizo los campos permitidos
            if (!string.IsNullOrWhiteSpace(dto.Estado))
                pedidoUpdate.Estado = dto.Estado;

            if (dto.IdMetodoPago.HasValue)
                pedidoUpdate.IdMetodoPago = dto.IdMetodoPago.Value;

            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido == null) return false;

            if (pedido.Estado != "Pendiente")
                throw new IndexOutOfRangeException("Solo se puede eliminar pedidos pendientes");

            _context.Pedido.Remove(pedido);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}