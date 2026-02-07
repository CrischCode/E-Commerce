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
                    Categoria = x.c.Nombre
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

        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _context.Producto
            .FirstOrDefaultAsync(p => p.IdProducto == id);
        }

        public async Task<Producto> CreateAsync(Producto producto)
        {
            //validar que el producto exista
            var exist = await _context.Producto.AnyAsync(p =>
                p.Nombre != null && producto.Nombre != null &&
                p.Nombre.ToLower() == producto.Nombre.ToLower());
            if (exist) throw new InvalidOperationException("Ya hay un producto con ese nombre");

            //validar que la categoriaa exista
            var categoriaExist = await _context.Categoria
            .AnyAsync(c => c.Id_Categoria == producto.IdCategoria);
            if (!categoriaExist) throw new KeyNotFoundException("La categoria no existe");

            _context.Producto.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<Producto> UpdateAsync(Producto producto)
        {
            //ver si el producto no exite
            var exist = await _context.Producto
            .AsNoTracking()
            .AnyAsync(p => p.IdProducto == producto.IdProducto);

            if (!exist)
                throw new KeyNotFoundException("El producto no existe");

            //ver si la categoria existe
            var categoriaExist = await _context.Categoria

            .AnyAsync(c => c.Id_Categoria == producto.IdCategoria);
            if (!categoriaExist) throw new KeyNotFoundException("La categoria no existe");

            await _context.SaveChangesAsync();
            return producto;
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