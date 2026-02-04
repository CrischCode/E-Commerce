using Ecommerce.API.DTOs;

namespace Ecommerce.API.Interfaces; 

public interface IPersonaService
{
    Task<(IEnumerable<PersonaReadDto> Items, int Total)> GetPagedAsync(int page, int pageSize, Guid? idPersona, string? busqueda);
    Task<PersonaReadDto?> GetByIdAsync(Guid id);
    Task<PersonaReadDto> CreateAsync(PersonaCreateDto dto);
    Task<bool> PatchAsync(Guid id, PersonaPatchDto dto);
    Task<bool> DeleteAsync(Guid id);
}