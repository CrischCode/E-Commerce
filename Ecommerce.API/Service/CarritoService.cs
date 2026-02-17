using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Interfaces;
using Ecommerce.API.Data;
using Ecommerce.API.Models;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Shared.DTOs;

namespace Ecommerce.API.Service
{
    public class CarritoService: ICarrito
    {
        private readonly AppDbContext _context;

        public CarritoService (AppDbContext context)
        {
            _context = context;
        }

        public async Task<CarritoReadDto?> GetCarritoByClienteAsync(int idCliente)
        {
            var carrito = await _context.Carrito
            .Include(c => c.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(c => c.IdCliente == idCliente);

            if(carrito == null) return null;

            return new CarritoReadDto {
            IdCarrito = carrito.IdCarrito,
            Items = carrito.Detalles.Select(d => new CarritoItemDto {
                IdProducto = d.IdProducto,
                NombreProducto = d.Producto?.Nombre ?? "Desconocido",
                Precio = d.Producto?.Precio ?? 0,
                Cantidad = d.Cantidad
            }).ToList()
        };
        }

        public async Task<bool> AgregarProductoAsync(int idCliente, int idProducto, int cantidad)
{
    //buscamos o creamos el carrito
    var carrito = await _context.Carrito
        .FirstOrDefaultAsync(c => c.IdCliente == idCliente);

    if (carrito == null)
    {
        carrito = new Carrito { IdCliente = idCliente };
        _context.Carrito.Add(carrito);
        await _context.SaveChangesAsync();
    }

    var detalle = await _context.DetalleCarrito
        .FirstOrDefaultAsync(d => d.IdCarrito == carrito.IdCarrito && d.IdProducto == idProducto);

    if (detalle != null)
    {
        detalle.Cantidad += cantidad; 
        if(detalle.Cantidad <= 0)
                {
                    _context.DetalleCarrito.Remove(detalle);
                }
    } 
    else
    {
        _context.DetalleCarrito.Add(new DetalleCarrito
        {
            IdCarrito = carrito.IdCarrito,
            IdProducto = idProducto,
            Cantidad = cantidad 
        });
    }

    carrito.FechaActualizacion = DateTime.Now;
    return await _context.SaveChangesAsync() > 0;
}

        public async  Task<bool> EliminarProductoAsync(int idCarrito, int idProducto)
        {
            var detalle = await _context.DetalleCarrito
            .FirstOrDefaultAsync(d => d.IdCarrito == idCarrito && d.IdProducto == idProducto);

            if(detalle == null) return false;
            _context.DetalleCarrito.Remove(detalle);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> LimpiarCarritoAsync(int idCliente)
        {
            var carrito = await _context.Carrito
            .Include(c => c.Detalles)
            .FirstOrDefaultAsync(c => c.IdCliente == idCliente);

            if(carrito == null) return false;

            _context.DetalleCarrito.RemoveRange(carrito.Detalles);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> ProcesarCompraAsync(ConfirmarPedidoDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            //aqui ontnemos el carrito con los productos 
            var carrito = await _context.Carrito
            .Include(c => c.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(c => c.IdCliente == dto.IdCliente);

            if(carrito == null || !carrito.Detalles.Any()) return false;

            //se crea la cabecera del pedido
            var nuevoPedido = new Pedido
            {
                IdCliente = dto.IdCliente,
                IdMetodoPago = dto.IdMetodoPago,
                FechaPedido = DateTime.Now,
                Estado = "Pendiente",
                Total = carrito.Detalles.Sum(d => d.Cantidad * (d.Producto?.Precio ?? 0))
            };

            //se guarda el pedido
            _context.Pedido.Add(nuevoPedido);
            await _context.SaveChangesAsync();

            //INVENTARIO
            //aqui movemois los detalles y afectamos el invenatario por si se hace la compra
            foreach(var item in carrito.Detalles)
            {
                var producto = item.Producto;
                if(producto == null || producto.Existencias < item.Cantidad)
                throw new Exception($"Stock insuficiente para: {producto?.Nombre}");

                //si hay stok creamos el detalle del pedido 
                var detalles = new DetallePedido {
                    IdPedido = nuevoPedido.IdPedido,
                    IdProducto = item.IdProducto,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = producto.Precio
                };
                _context.DetallePedido.Add(detalles);

            //Actualizamos el inventario
            producto.Existencias -= item.Cantidad;

            //REGISTRAMOS EL MOVIMIENTO
            _context.MovimientoInventario.Add(new MovimientoInventario
            {
                IdProducto = item.IdProducto,
                TipoMovimiento = "Salida",
                Cantidad = item.Cantidad,
                Motivo = $"Venta Pedido #{nuevoPedido.IdPedido}",
                FechaMovimiento = DateTime.Now
            });
            }  

             //limpiar carrito
            _context.DetalleCarrito.RemoveRange(carrito.Detalles);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true; 
             
        }
        
    }
}