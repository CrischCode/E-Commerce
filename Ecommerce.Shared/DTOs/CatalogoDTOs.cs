using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOs
{
    public class ProductoCatalogoResponse<T>
    {
        public int Total { get; set; }
        public List<T> Data { get; set; } = new();
    }

    public class CategoriaDto
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
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