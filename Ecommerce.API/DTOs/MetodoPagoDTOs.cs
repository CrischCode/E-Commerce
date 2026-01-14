using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.API.DTOs
{
    public class MetodoPagoReadDTOs
    {
        public int IdMetodoPago {get; set;}
        public string? Nombre {get; set;}
        
    }

    public class MetodoPagoCreateDTOs
    {
        public string? Nombre {get; set;}
        
    }

    public class MetodoPagoUpdateDTOs
    {
        public string? Nombre {get; set;}
        
    }
}