using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Shared.DTOs
{
    public class CarritoReadDto
    {
        public int IdCarrito {get; set;}
        public int IdCliente { get; set; }
        public List<CarritoItemDto> Items { get; set; } = new List<CarritoItemDto>();
    }
    public class CarritoDetalleDto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubTotal => Cantidad * PrecioUnitario;
    }

    public class AgregarProductoDto
    {
        public int IdCliente {get; set;}
        public int IdProducto {get; set;}
        public int Cantidad {get; set;}
    }

    public class CarritoItemDto
    {
        public int IdProducto {get; set;}
        public string NombreProducto { get; set; } = string.Empty;
        public decimal Precio {get; set;}
        public int Cantidad {get; set;}
        public decimal SubTotal => Precio * Cantidad;
    }

    public class ConfirmarPedidoDto
    {
        public int IdCliente {get; set;}
        public int IdMetodoPago {get; set;}
    }
}