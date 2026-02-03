using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.DTOs;
using Ecommerce.API.Models;

namespace Ecommerce.API.Interfaces
{
    public interface IMovimientoInventario
    {
        Task<(IEnumerable<MovimientoReadDto> Items, int Total)> GetPagedAsync(int page, int pageSize); 
        Task<bool> CreateMovimientoAsync( MovimientoCreateDto dto);
    }
}