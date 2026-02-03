using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.API.Models
{
    public class Pedido
    {
      [Key]
      [Column("id_pedido")]
      public int IdPedido {get; set;}
      
      [Column("id_cliente")]
      public int IdCliente {get; set;}

      [Column("id_metodo_pago")]
      public int IdMetodoPago {get; set;}

      [Column("fecha_pedido")]
      public DateTime FechaPedido{get; set;} = DateTime.UtcNow;

      [Column("total")]
      public decimal Total {get; set;}

      [Column("estado")]
      public string? Estado {get; set;} = "Pendiente";

       [ForeignKey("IdCliente")]
       public virtual Cliente? Cliente {get; set;}

        [ForeignKey("IdMetodoPago")]
        public virtual MetodoPago? MetodoPago {get; set;}

      public ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();

    }
}