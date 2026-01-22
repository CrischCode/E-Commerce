using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.DTOs;
using Ecommerce.API.Models;

namespace Ecommerce.API.Interfaces
{
    public interface ICliente
    {
        Task<(IEnumerable<ClienteReadDto> Items, int Total)> GetPagedAsync(int page, int pageSize); 
        Task<ClienteReadDto?> GetByIdAsync(int id);
        Task<Cliente> CreateAsync(ClienteCreateDto dto);
        Task<bool> UpdateAsync(int id, ClienteUpdateDto dto);
        Task<bool> DeleteAsync(int id); 

        Task<bool> RegistrarUsuarioCompletoAsync(RegistroDto dto);
        
    }
}