using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Models;

namespace Ecommerce.API.Interfaces
{
    public interface IDetallePedido
    {
        Task<IEnumerable<DetallePedido>> GetAllAsync();
        Task<DetallePedido> GetByIdAsync(int id);
        Task<DetallePedido> CreateAsync(DetallePedido detallePedido);
        Task<DetallePedido> UpdateAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}