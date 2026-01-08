using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.API.DTOs
{
    public class ProductoCreateDtos
    {
    public int IdProducto {get; set;}
    public string? Nombre {get; set;} = null!;
    public decimal Precio {get; set;}
    public int Existencias {get; set;}
    public int IdCategoria {get; set;}
    }

    public class ProductoReadteDtos
    {
    public int IdProducto {get; set;}
    public string? Nombre {get; set;} = null!;
    public decimal Precio {get; set;}
    public int Existencias {get; set;}
    public int IdCategoria {get; set;}
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
}