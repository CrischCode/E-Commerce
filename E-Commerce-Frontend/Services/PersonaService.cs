using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Ecommerce.Shared.DTOs;
using E_Commerce_Frontend.Interfaces;

namespace E_Commerce_Frontend.Services
{
    public class PersonaService: IPersonaService
    {
        private readonly HttpClient _http;
        public PersonaService(HttpClient http) => _http = http;   

        public async Task<PersonaPagedResponse> GetPagedAsync(int page, int pageSize, string? busqueda = null)
        {
            var url = $"api/Persona?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(busqueda)) url += $"&busqueda={busqueda}";
            
            return await _http.GetFromJsonAsync<PersonaPagedResponse>(url) ?? new();
        } 

        public async Task<PersonaReadDto?> GetByIdAsync(Guid id) =>
        await _http.GetFromJsonAsync<PersonaReadDto>($"api/Persona/{id}");

        public async Task<bool> CreateAsync(PersonaCreateDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/Persona", dto);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Guid id, PersonaPatchDto dto)
        {
            var res = await _http.PatchAsJsonAsync($"api/Persona/{id}", dto);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var res = await _http.DeleteAsync($"api/Persona/{id}");
            return res.IsSuccessStatusCode;
        }
    }

    
}