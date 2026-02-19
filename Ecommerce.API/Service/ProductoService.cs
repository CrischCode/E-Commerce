using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ecommerce.API.Data;
using Ecommerce.API.Interfaces;
using Ecommerce.API.Models;
using Ecommerce.Shared.DTOs;

namespace Ecommerce.API.Service
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<ProductoReadtDtos> Items, int Total)> GetPagedAsync(int page, int pageSize, string? categoria, string? busqueda)
        {
            var query = from p in _context.Producto
                        join c in _context.Categoria on p.IdCategoria equals c.Id_Categoria
                        select new { p, c };

            if (!string.IsNullOrWhiteSpace(categoria))
            {
                query = query.Where(x => x.c.Nombre != null && x.c.Nombre.ToLower().Contains(categoria.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                query = query.Where(x => x.p.Nombre != null && x.p.Nombre.ToLower().Contains(busqueda.ToLower()));
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.p.IdProducto)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProductoReadtDtos
                {
                    IdProducto = x.p.IdProducto,
                    Nombre = x.p.Nombre,
                    Precio = x.p.Precio,
                    Existencias = x.p.Existencias,
                    IdCategoria = x.p.IdCategoria,
                    Categoria = x.c.Nombre,
                    FotoData = x.p.FotoData,
                    FotoMimeType = x.p.FotoMimeType
                })
                .ToListAsync();

            return (items, total);
        }

        /*
            public async Task<IEnumerable<Producto>> GetAllAsync()
            {
                  return await _context.Producto
                  .AsNoTracking()
                  .ToListAsync();
            } */

        public async Task<ProductoReadtDtos?> GetByIdAsync(int id)
        {
            return await _context.Producto
            .Where(p => p.IdProducto == id)
            .Select(p => new ProductoReadtDtos
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Existencias = p.Existencias,
                IdCategoria = p.IdCategoria,
                FotoData = p.FotoData,
                FotoMimeType = p.FotoMimeType
            }).FirstOrDefaultAsync();
        }

        public async Task<ProductoReadtDtos> CreateAsync(ProductoCreateDtos dto)
        {
            //validar que el producto exista
          var exit = await _context.Producto.AnyAsync(p => p.Nombre != null && dto.Nombre != null
          && p.Nombre.ToLower() == dto.Nombre.ToLower());
          if(exit) throw new InvalidOperationException("Ya hay un producto con ese nombre");

            //validar que la categoriaa exista
            var categoriaExist = await _context.Categoria
            .AnyAsync(c => c.Id_Categoria == dto.IdCategoria);
            if (!categoriaExist) throw new KeyNotFoundException("La categoria no existe");

            //se crea la entidad en la bd
            var productoNuevo = new Producto
            {
                Nombre = dto.Nombre,
            Precio = dto.Precio,
            Existencias = dto.Existencias,
            IdCategoria = dto.IdCategoria,
            FotoData = dto.FotoData,
            FotoMimeType = dto.FotoMimeType
            };

            _context.Producto.Add(productoNuevo);
            await _context.SaveChangesAsync();

            return new ProductoReadtDtos
            {
                IdProducto = productoNuevo.IdProducto,
                Nombre = productoNuevo.Nombre,
                Precio = productoNuevo.Precio,
                Existencias = productoNuevo.Existencias,
                IdCategoria = productoNuevo.IdCategoria,
                FotoData = productoNuevo.FotoData,
                FotoMimeType = productoNuevo.FotoMimeType
            };
        }

        public async Task<bool> UpdateAsync(int id, ProductoUpdateDto dto)
        {
            var producto = await _context.Producto.FindAsync(id);
            if(producto == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Nombre)) producto.Nombre = dto.Nombre;
        if (dto.Precio.HasValue) producto.Precio = dto.Precio.Value;
        if (dto.Existencias.HasValue) producto.Existencias = dto.Existencias.Value;
        if (dto.IdCategoria.HasValue) producto.IdCategoria = dto.IdCategoria.Value;
        //actualizacion de la imagen
        if (dto.FotoData != null) {
            producto.FotoData = dto.FotoData;
            producto.FotoMimeType = dto.FotoMimeType;
        }

        return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto == null) return false;

            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CategoriaExistAsync(int categoriaId)
        {
            return await _context.Categoria
                .AnyAsync(c => c.Id_Categoria == categoriaId);
        }
    }

}