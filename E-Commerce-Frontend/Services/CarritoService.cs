using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Shared.DTOs;
using E_Commerce_Frontend.Interfaces;
using System.Net.Http.Json;

namespace E_Commerce_Frontend.Services
{
    public class CarritoService : ICarrito 
    {
        
        private readonly HttpClient _http;

        public CarritoService(HttpClient http) => _http = http;

        public async Task<CarritoReadDto?> GetCarritoByClienteAsync(int idCliente)
        {
            return await _http.GetFromJsonAsync<CarritoReadDto>($"api/Carrito/{idCliente}");
        }

        public async Task<bool> AgregarProductoAsync(int idCliente, int idProducto, int cantidad)
        {
            var dto = new AgregarProductoDto{IdCliente = idCliente, IdProducto = idProducto, Cantidad = cantidad};
            var response = await _http.PostAsJsonAsync($"api/Carrito/agregar", dto);
            return response.IsSuccessStatusCode; {}
        }

        public async Task<bool> EliminarProductoAsync(int idCarrito, int idProducto)
        {
            var response = await _http.DeleteAsync($"api/Carrito/eliminar/{idCarrito}/{idProducto}");
            return response.IsSuccessStatusCode; {}
        }

        public async Task<bool> LimpiarCarritoAsync(int idCliente)
        {
            var response = await _http.DeleteAsync($"api/Carrito/limpiar/{idCliente}");
            return response.IsSuccessStatusCode; 
        }

        public async Task<bool> ProcesarCompraAsync(ConfirmarPedidoDto dto)
        {
            var response = await _http.PostAsJsonAsync($"api/Carrito/nuevo", dto);
            return response.IsSuccessStatusCode;
        }
        
    }
}