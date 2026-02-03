using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.API.Models
{
    [Table("detalle_pedido")]
    public class DetallePedido
    {
        [Key]
        [Column("id_detalle_pedido")]
        public int IdDetallePedido {get; set;}
        [Column("id_pedido")]
        public int IdPedido {get; set;}

        [Column("id_producto")]
        public int IdProducto {get; set;}

        [Column("cantidad")]
        public int Cantidad {get; set;}

        [Column("precio_unitario")]
        public decimal PrecioUnitario {get; set;}

              [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("sub_total")]
        public decimal SubTotal {get; private set;}

        //para navegacion
        public Pedido Pedido { get; set; } = null!;
        public Producto Producto { get; set; } = null!;
        
    }
}