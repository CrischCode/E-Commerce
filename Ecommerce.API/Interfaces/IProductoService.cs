using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
//using Ecommerce.API.DTOs;
using Ecommerce.Shared.DTOs;
using Ecommerce.API.Models;

namespace Ecommerce.API.Interfaces
{
    public interface IProductoService
    {
        Task<(IEnumerable<ProductoReadtDtos> Items, int Total)> GetPagedAsync(int page, int pageSize, string? categoria, string? busqueda);
        Task<ProductoReadtDtos?> GetByIdAsync(int id);
        Task<ProductoReadtDtos> CreateAsync(ProductoCreateDtos dto);
        Task<bool> UpdateAsync(int id, ProductoUpdateDto dto);
        Task<bool> DeleteAsync(int id);  
        Task<bool> CategoriaExistAsync (int Id_Categoria);  
    }
}