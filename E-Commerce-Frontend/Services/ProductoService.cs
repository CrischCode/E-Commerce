using System.Net.Http.Json;
using Ecommerce.Shared.DTOs;
using E_Commerce_Frontend.Interfaces;

namespace E_Commerce_Frontend.Services
{
    public class ProductoService : IProductoService
    {
        private readonly HttpClient _http;
        public ProductoService(HttpClient http) => _http = http;

        public async Task<PagedResponse<ProductoReadtDtos>> GetPagedAsync(int page, int pageSize, string? categoria = null)
        {
            var url = $"api/Producto/paged?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrEmpty(categoria)) url += $"&categoria={categoria}";
            return await _http.GetFromJsonAsync<PagedResponse<ProductoReadtDtos>>(url) ?? new();
        }

        public async Task<ProductoReadtDtos?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<ProductoReadtDtos>($"api/Producto/{id}");

        public async Task<bool> CreateAsync(ProductoCreateteDtos dto)
        {
            var res = await _http.PostAsJsonAsync("api/Producto", dto);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, ProductoUpdateDto dto)
        {
            var res = await _http.PatchAsJsonAsync($"api/Producto/{id}", dto);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var res = await _http.DeleteAsync($"api/Producto/{id}");
            return res.IsSuccessStatusCode;
        }
    }
}