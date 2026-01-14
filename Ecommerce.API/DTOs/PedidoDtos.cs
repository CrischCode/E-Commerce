using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.API.DTOs
{
    public class PedidoReadDto
    {
      public int IdPedido {get; set;}
      public int IdCliente {get; set;}
      public int IdMetodoPago {get; set;}
      public DateOnly FechaPedido{get; set;}
      public decimal Total {get; set;}
      public string? Estado {get; set;} 
    }

    public class PedidoCreateDto
    {
      public int IdCliente {get; set;}
      public int IdMetodoPago {get; set;}
     // public DateOnly? FechaPedido{get; set;}
     // public decimal Total {get; set;}
     // public string? Estado {get; set;}
     public List<DetallePedidoCreateDto> Detalles {get; set;} = null!;
    }

    public class PedidoUpdateDto
    {
      public int IdCliente {get; set;}
      public int? IdMetodoPago {get; set;}
      public DateOnly? FechaPedido{get; set;}
      public decimal Total {get; set;}
      public string? Estado {get; set;} 
    }
}