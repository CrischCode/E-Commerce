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
}