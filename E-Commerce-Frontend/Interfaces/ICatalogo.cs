using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Shared.DTOs; 

namespace E_Commerce_Frontend.Interfaces
{
    public interface ICatalogo
    {
        Task<ProductoCatalogoResponse<ProductoReadClienteDto>> GetPagedAsync(int page, int pageSize, string? buscar, string? categoria);
        Task<List<CategoriaDto>> GetCategoriasAsync();
        
    }
}