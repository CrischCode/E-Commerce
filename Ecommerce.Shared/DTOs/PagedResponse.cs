using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOs
{
    public class PagedResponse<T>
    {
       public List<T> Items {get; set;} = new();
       public int TotalCount {get; set;}
       public int CurrentPage {get; set;}
       public int PageSize {get; set;}
    }

    //Dto de Paginado de Personas
    public class PersonaPagedResponse
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<PersonaReadDto> Data { get; set; } = new();
    }

    public class PedidoPagedResponse
    {
        
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<PedidoReadDto> Data { get; set; } = new();
        
    }
}