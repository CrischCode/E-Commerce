using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Models;

namespace Ecommerce.API.Interfaces
{
    public interface ICategoria
    {
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria?> GetByIdAsync(int id);
    Task<Categoria> CreateAsync(Categoria categoria);

    //Task<Persona> SaveChangesAsync(Persona persona);
    Task UpdateAsync(Categoria categoria);
    Task<bool> DeleteAsync(int id);
        
    }
}