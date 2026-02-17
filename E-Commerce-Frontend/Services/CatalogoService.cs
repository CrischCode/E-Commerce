using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Shared.DTOs;
using E_Commerce_Frontend.Interfaces;
using System.Net.Http.Json;

namespace E_Commerce_Frontend.Services
{
    public class CatalogoService : ICatalogo
    {
        private readonly HttpClient _http;

        public CatalogoService(HttpClient http) => _http = http;
        public async Task<ProductoCatalogoResponse<ProductoReadClienteDto>> GetPagedAsync(int page, int pageSize, string? busqueda, string? categoria)
        {
            var url = $"api/CatalogoCliente?paged?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrEmpty(busqueda)) url += $"&buscar={Uri.EscapeDataString(busqueda)}";
        if (!string.IsNullOrEmpty(categoria)) url += $"&categoria={Uri.EscapeDataString(categoria)}";

        return await _http.GetFromJsonAsync<ProductoCatalogoResponse<ProductoReadClienteDto>>(url);
        }

        public async Task<List<CategoriaDto>> GetCategoriasAsync() 
        {
            return await _http.GetFromJsonAsync<List<CategoriaDto>>("api/Categoria");
        }
        
    }
}