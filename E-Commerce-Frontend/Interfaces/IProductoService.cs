using Ecommerce.Shared.DTOs;

namespace E_Commerce_Frontend.Interfaces
{
    public interface IProductoService
    {
        Task<PagedResponse<ProductoReadtDtos>> GetPagedAsync(int page, int pageSize, string? categoria = null, string? busqueda = null);
        Task<ProductoReadtDtos?> GetByIdAsync(int id);
        Task<bool> CreateAsync(ProductoCreateteDtos dto);
        Task<bool> UpdateAsync(int id, ProductoUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}