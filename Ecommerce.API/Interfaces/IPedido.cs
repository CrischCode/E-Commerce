using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Shared.DTOs;
using Ecommerce.API.Models;

namespace Ecommerce.API.Interfaces
{
    public interface IPedido
    {
        Task<(IEnumerable<PedidoReadDto> Items, int Total)> GetPagedAsync(int page, int pageSize, string? estado, int? idPedido);
        Task<Pedido?> GetByIdAsync(int id);
        Task<Pedido?> CreateAsync(Pedido pedido);
        Task<bool> UpdateAsync(int id, PedidoUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}