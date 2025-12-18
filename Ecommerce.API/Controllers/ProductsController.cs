using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new[]
            {
                new { Id = 1, Name = "Laptop", Price = 1200 },
                new { Id = 2, Name = "Mouse", Price = 25 }
            });
        }
    }
}
