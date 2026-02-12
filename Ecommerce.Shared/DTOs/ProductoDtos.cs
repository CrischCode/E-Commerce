using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOs
{
    public class ProductoCreateDtos
    {
    public int IdProducto {get; set;}
    public string? Nombre {get; set;} = null!;
    public decimal Precio {get; set;}
    public int Existencias {get; set;}
    public int IdCategoria {get; set;} 
    }

    public class ProductoReadtDtos
    {
    public int IdProducto {get; set;}
    public string? Nombre {get; set;} = null!;
    public decimal Precio {get; set;}
    public int Existencias {get; set;}
    public int IdCategoria {get; set;}
    public string? Categoria {get; set;}
    }

     public class ProductoCreateteDtos
    {
    public string? Nombre {get; set;}
    public decimal Precio {get; set;}
    public int Existencias {get; set;}
    public int IdCategoria {get; set;}
    }

    public class ProductoUpdateDto
    {
    public string? Nombre {get; set;}
    public decimal? Precio {get; set;}
    public int? Existencias {get; set;}
    public int? IdCategoria {get; set;}
    }

    public class ProductoReadClienteDto
    {
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public string? CategoriaNombre { get; set; }
    public bool TieneStock => Existencias > 0;
    public int Existencias {get; set;}
    //public double PromedioCalificacion {get; set;}
    public string? UrlImage {get; set;}

    }
}