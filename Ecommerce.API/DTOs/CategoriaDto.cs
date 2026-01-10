using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.API.DTOs
{
    public class CategoriaReadDto
    {
        public int Id_Categoria {get; set;}
        public string Nombre {get; set;} = null!;
        public string Descripcion {get; set;} = null!; 
        
    }

    public class CategoriaCreateDto
    {
        public string Nombre {get; set;} = null!;
        public string Descripcion {get; set;} = null!; 
        
    }

    public class CategoriaUpdateDto
    {
        public string Nombre {get; set;} = null!;
        public string Descripcion {get; set;} = null!; 
        
    }

    public class CategoriaDeleteDto
    {
        public string Nombre {get; set;} = null!;
        public string Descripcion {get; set;} = null!; 
        
    }
}