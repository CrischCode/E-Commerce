using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.API.Models;

[Table("carrito")]
public class Carrito
{
    [Key]
    [Column("id_carrito")]
    public int IdCarrito {get; set;}

    [Column("id_cliente")]
    public int IdCliente {get; set;}

    [Column("fecha_creacion")]
    public DateTime FechaCreacion {get; set;}

    [Column("fecha_actualizacion")]
    public DateTime? FechaActualizacion {get; set;}

    public virtual ICollection<DetalleCarrito> Detalles {get; set;} = new List<DetalleCarrito>();
    
}

public class DetalleCarrito
{
    [Key]
    [Column("id_detallecarrito")]
    public int IdDetalleCarrito {get; set;}
    [Column("id_carrito")]
    public int IdCarrito {get; set;}
    [Column("id_producto")]
    public int IdProducto {get; set;}
    [Column("cantidad")]
    public int Cantidad {get; set;}

    [ForeignKey("IdCarrito")]
    public virtual Carrito? Carrito {get; set;}

    [ForeignKey("IdProducto")]
    public virtual Producto? Producto {get; set;}
}


    