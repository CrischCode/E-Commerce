using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Ecommerce.API.Models
{
    public class MetodoPago
    {
        [Key]
        [Column("id_metodo")]
        public int IdMetodoPago {get; set;}

        [Column("nombre")]
        public string? Nombre {get; set;}
    }
}