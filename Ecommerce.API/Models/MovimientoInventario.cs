using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.API.Models 

{
    [Table("movimiento_inventario", Schema = "public")]
    public class MovimientoInventario
    {
        [Key]
        [Column("id_movimiento")]
        public int IdMovimiento {get; set;}

        [Column("id_producto")]
        public int IdProducto {get; set;}

        [Column("tipo_movimiento")]
        public string TipoMovimiento {get; set;} = null!;

        [Column("cantidad")]
        public int Cantidad {get; set;}

        [Column("motivo")]
        public string? Motivo {get; set;}

        [Column("fecha_movimiento")]
        public DateTime FechaMovimiento {get; set;} = DateTime.Now;

        [Column("id_empleado")]
        public int? IdEmpleado {get; set;}


    }
}
