using Ecommerce.API.Models;

namespace Ecommerce.API.Interfaces; 

public interface IPersonaService
{
    Task<IEnumerable<Persona>> GetAllAsync();
    Task<Persona?> GetByIdAsync(Guid id);
    Task<Persona> CreateAsync(Persona persona);

    //Task<Persona> SaveChangesAsync(Persona persona);
    Task UpdateAsync(Persona persona);
    Task<bool> DeleteAsync(Guid id);
}
