using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOs
{
    public class DetallePedidoReadDto
    {
        public int IdDetallePedido {get; set;}
        public int IdProducto {get; set;}
        public string NombreProducto {get; set;} = string.Empty;
        public int Cantidad {get; set;}
        public decimal PrecioUnitario {get; set;}
        public decimal SubTotal {get; set;}
    }

    public class DetallePedidoCreateDto
    {
        [Required]
        public int IdProducto {get; set;}

        [Range(1, 1000, ErrorMessage = "La cantidad debe de ser entre 1 a 100")]
        public int Cantidad {get; set;}
       // public decimal SubTotal {get; set;}
    }
}