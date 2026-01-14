using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Models;

namespace Ecommerce.API.Interfaces
{
    public interface IPedido
    {
        Task<(IEnumerable<Pedido> Items, int Total)> GetPagedAsync(int page, int pageSize);
        Task<Pedido?> GetByIdAsync(int id);
        Task<Pedido> CreateAsync(Pedido pedido);
        Task<Pedido> UpdateAsync(Pedido pedido);
        Task<bool> DeleteAsync(int id);
    }
}