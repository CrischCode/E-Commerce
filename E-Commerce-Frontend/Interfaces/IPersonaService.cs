using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Shared.DTOs;

namespace E_Commerce_Frontend.Interfaces
{
    public interface IPersonaService
    {
        Task<PersonaPagedResponse> GetPagedAsync(int page, int pageSize, string? busqueda = null);
        Task<PersonaReadDto?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(PersonaCreateDto dto);
        Task<bool> UpdateAsync(Guid id, PersonaPatchDto dto);
        Task<bool> DeleteAsync(Guid id);
        
    }
}