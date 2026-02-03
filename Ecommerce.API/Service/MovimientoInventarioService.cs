using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Interfaces;
using Ecommerce.API.Data;
using Ecommerce.API.Models;
using Microsoft.EntityFrameworkCore;
using Ecommerce.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Service
{
    public class MovimientoInventarioService: IMovimientoInventario
    {
        private readonly AppDbContext _context;

        public MovimientoInventarioService(AppDbContext context)
        {
            _context = context;
        }

    public async Task<(IEnumerable<MovimientoReadDto> Items, int Total)> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.MovimientoInventario.AsNoTracking();
            var total = await query.CountAsync();

            var item = await query
            .OrderByDescending(m => m.FechaMovimiento)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MovimientoReadDto
            {
                IdMovimiento = m.IdMovimiento,
                NombreProducto = _context.Producto.First(p => p.IdProducto == m.IdProducto).Nombre,
                TipoMovimiento = m.TipoMovimiento,
                Cantidad = m.Cantidad,
                Motivo = m.Motivo,
                FechaMovimiento = m.FechaMovimiento
            })
            .ToListAsync();

            return(item, total);
        }

    public async Task<bool> CreateMovimientoAsync(MovimientoCreateDto dto)
        {
           var producto = await _context.Producto.FindAsync(dto.IdProducto);
           if(producto == null) return false;

           //aqui podemos ajustar el stock en la tabla si queremos
           if(dto.TipoMovimiento == "Entrada")
           producto.Existencias += dto.Cantidad;
           else if(dto.TipoMovimiento == "Salida")
           producto.Existencias -= dto.Cantidad;

           var movimiento = new  MovimientoInventario {
               IdProducto = dto.IdProducto,
               TipoMovimiento = dto.TipoMovimiento,
               Cantidad = dto.Cantidad,
               Motivo = "(Ajuste manual) " + dto.Motivo,
               FechaMovimiento = DateTime.Now ,
               IdEmpleado = null
            };

            _context.MovimientoInventario.Add(movimiento);
            return await _context.SaveChangesAsync() > 0;
        }
        
    }
}