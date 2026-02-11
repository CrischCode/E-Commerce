using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOs
{
    public class PedidoReadDto
    {
      public int IdPedido {get; set;}
      public int IdCliente {get; set;}
      public string Cliente {get; set;} = string.Empty;
      public int IdMetodoPago {get; set;}
      public string? MetodoPago {get; set;} = string.Empty;
      public DateOnly FechaPedido{get; set;}
      public decimal Total {get; set;}
      public string? Estado {get; set;}
      //public decimal? SubTotal {get; set;} 

      public List<DetallePedidoReadDto> Detalles { get; set; } = new();
    }

    public class PedidoCreateDto
    {
      public int IdCliente {get; set;}
      public int IdMetodoPago {get; set;}

      [Required]
      [MinLength(1, ErrorMessage = "El pedido debe de tener almenos un producto.")]
     public List<DetallePedidoCreateDto> Detalles {get; set;} = null!;
    }

    public class PedidoUpdateDto
    {
      public int? IdMetodoPago {get; set;}
      public string? Estado {get; set;} 

     // public List<DetallePedidoReadDto> Detalle {get; set;} = new();
    }

    //Paginado de Pedidos
     public class PedidoResponse<T>
        {
            public int Total {get; set;}
            public List<PedidoReadDto> Date {get; set;} = new();
        } 
}