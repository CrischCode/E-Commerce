using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.API.DTOs
{
    public class DetallePedidoReadDto
    {
        public int IdDetallePedido {get; set;}
        public int IdPedido {get; set;}
        public int IdProducto {get; set;}
        public int Cantidad {get; set;}
        public decimal PrecioUnitario {get; set;}
        public decimal SubTotal {get; set;}
    }

    public class DetallePedidoCreateDto
    {
        public int IdProducto {get; set;}
        public int Cantidad {get; set;}
        public decimal PrecioUnitario {get; set;}
       // public decimal SubTotal {get; set;}
    }
}