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
    public class CatalogoClienteService: ICatalogoCliente
    {
        private readonly AppDbContext _context;

        public CatalogoClienteService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<(IEnumerable<ProductoReadClienteDto> Items, int Total)> GetPagedAsync(int page, int pageSize, string? busqueda, string? categoria)
        {
            var query = _context.Producto
            .Include(p => p.Categoria)
            .AsQueryable();

            //filtro por nombre 
            if(!string.IsNullOrWhiteSpace(busqueda)) {
            query = query.Where(p => p.Nombre.Contains(busqueda));
            }

            //filtro por nombre 
            if(!string.IsNullOrWhiteSpace(categoria)) {
            query = query.Where(p => p.Categoria.Nombre.Contains(categoria));
            }

            //se obtiene le total
            int total = await query.CountAsync();

            //paginacion
            var items = await query
            .Skip((page-1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductoReadClienteDto
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                //Descripcion
                Precio = p.Precio,
                Existencias = p.Existencias,
                CategoriaNombre = p.Categoria != null ? p.Categoria.Nombre : "Sin Categoría",
                //UrlImage
            }).ToListAsync();

            return (items, total);
        }
    }
}