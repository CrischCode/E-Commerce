using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Shared.DTOs; 

namespace E_Commerce_Frontend.Interfaces
{
    public interface IPedidos
    {
        
        Task<PedidoResponse<PedidoReadDto>> GetPagedAsync(int page, int pageSize, string? estado = null, int? idPedido = null);
        Task<PedidoReadDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(PedidoCreateDto dto);
        Task<bool> UpdateAsync(int id, PedidoUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        
    }
}