using Ecommerce.Shared.DTOs;
using System.Net.Http.Json;

namespace E_Commerce_Frontend.Services
{
    public class ClienteClientService
    {
        private readonly HttpClient _http;

        public ClienteClientService(HttpClient http) => _http = http;

        public async Task<List<ClienteReadDto>> GetClientesAsync()
        {
            var response = await _http.GetFromJsonAsync<PagedResponse<ClienteReadDto>>("api/Cliente");
            return response?.Items ?? new List<ClienteReadDto>();
        }

        public async Task<bool> RegistrarClienteAsync(RegistroDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/Cliente/registro", dto);
            return response.IsSuccessStatusCode;
        }
    }
}