using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
//using Ecommerce.API.DTOs;
using Ecommerce.Shared.DTOs;
using Ecommerce.API.Models;

namespace Ecommerce.API.Interfaces; 

public interface IPersonaService
{
    Task<(IEnumerable<PersonaReadDto> Items, int Total)> GetPagedAsync(int page, int pageSize, Guid? idPersona, string? busqueda);
    Task<PersonaReadDto?> GetByIdAsync(Guid id);
    Task<PersonaReadDto> CreateAsync(PersonaCreateDto dto);
    Task<bool> PatchAsync(Guid id, PersonaPatchDto dto);
    Task<bool> DeleteAsync(Guid id);
}