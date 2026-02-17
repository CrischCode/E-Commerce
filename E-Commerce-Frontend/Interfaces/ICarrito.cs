using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Shared.DTOs; 

namespace E_Commerce_Frontend.Interfaces
{
    public interface ICarrito
    {
        Task<CarritoReadDto?> GetCarritoByClienteAsync(int idCliente);
        Task<bool> AgregarProductoAsync(int idCliente, int idProducto, int cantidad);
        Task<bool> EliminarProductoAsync(int idCarrito, int idProducto);
        Task<bool> LimpiarCarritoAsync(int idCliente);
        Task<bool> ProcesarCompraAsync(ConfirmarPedidoDto dto);
    }
}