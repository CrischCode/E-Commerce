using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Shared.DTOs;
using E_Commerce_Frontend.Interfaces;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;

namespace E_Commerce_Frontend.Services
{
    public class PedidoService: IPedidos
    {
        private readonly HttpClient _http;
        public PedidoService(HttpClient http) => _http = http; 

        public async Task<PedidoResponse<PedidoReadDto>> GetPagedAsync(int page, int pageSize, string? estado, int? idPedido)
        {
            var url = $"api/Pedido/paged?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrEmpty(estado)) url += $"&Estado={estado}";
            if(idPedido.HasValue) url += $"&idPedido={idPedido}";

            var response = await _http.GetFromJsonAsync<PedidoResponse<PedidoReadDto>>(url);
            return response ?? new PedidoResponse<PedidoReadDto>();
            
            
        }

        public async Task<PedidoReadDto?> GetByIdAsync(int id) {
        return await _http.GetFromJsonAsync<PedidoReadDto>($"api/Pedido/{id}");
        }

        public async Task<bool> CreateAsync(PedidoCreateDto dto)
        {
            var res = await _http.PostAsJsonAsync($"api/Pedido", dto);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, PedidoUpdateDto dto)
        {
            var res = await _http.PatchAsJsonAsync($"api/Pedido/{id}", dto);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var res = await _http.DeleteAsync($"api/Pedido/{id}");
            return res.IsSuccessStatusCode;
        }
    }
}