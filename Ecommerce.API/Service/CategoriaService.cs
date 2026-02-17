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
    public class CategoriaService: ICategoria
    {
    private readonly AppDbContext _context;
    
    public CategoriaService(AppDbContext context)
    {
        _context = context;
    }  

    public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            return await _context.Categoria
            .AsNoTracking()
            .ToListAsync();
        } 
    
    public async Task<Categoria?> GetByIdAsync(int id)
        {
            return await _context.Categoria
            .FirstOrDefaultAsync(c => c.Id_Categoria == id);
        }

    public async Task<Categoria> CreateAsync(Categoria categoria)
        {
            _context.Categoria.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }
    
    public async Task UpdateAsync(Categoria categoria)
        {
            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
        }
    
    public async Task<bool> DeleteAsync(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            if(categoria == null) return false;

            _context.Categoria.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }
    
    public async Task<bool> CategoriaExist(int Id_Categoria)
        {
            return await _context.Categoria
            .AnyAsync(c => c.Id_Categoria == Id_Categoria);
        }
        
    }
}