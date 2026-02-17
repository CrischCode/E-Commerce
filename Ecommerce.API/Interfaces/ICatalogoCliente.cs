using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Models;
using Ecommerce.Shared.DTOs;

namespace Ecommerce.API.Interfaces
{
    public interface ICatalogoCliente
    {
        Task<(IEnumerable<ProductoReadClienteDto> Items, int Total)> GetPagedAsync(int page, int pageSize, string? busqueda, string? categoria); 
    }
}