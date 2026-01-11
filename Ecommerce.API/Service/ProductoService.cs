using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ecommerce.API.Data;
using Ecommerce.API.Interfaces;
using Ecommerce.API.Models;

namespace Ecommerce.API.Service
{
    public class ProductoService: IProductoService
    {
    private readonly AppDbContext _context;
    
    public ProductoService(AppDbContext context)
    {
        _context = context;
    } 

    public async Task<IEnumerable<Producto>> GetAllAsync()
    {
          return await _context.Producto
          .AsNoTracking()
          .ToListAsync();
    } 

    public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _context.Producto
            .FirstOrDefaultAsync(p => p.IdProducto == id);
        }
    
    public async Task<Producto> CreateAsync(Producto producto)
        {
            var categoriaExist = await _context.Categoria
            .AnyAsync(c => c.Id_Categoria == producto.IdCategoria);
            if(!categoriaExist) throw new KeyNotFoundException("La categoria no existe");

            _context.Producto.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

    public async Task<Producto> UpdateAsync(Producto producto)
        {
            var categoriaExist = await _context.Categoria
            .AnyAsync(c => c.Id_Categoria == producto.IdCategoria);
            if(!categoriaExist) throw new KeyNotFoundException("La categoria no existe");

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