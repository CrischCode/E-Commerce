using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
//using Ecommerce.API.DTOs;
using Ecommerce.Shared.DTOs;
using Ecommerce.API.Interfaces;
using Ecommerce.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        /*
                [HttpGet]
                public async Task<IActionResult> GetAll()
                {
                    var producto = await _productoService.GetAllAsync();
                    var result = producto.Select(p => new ProductoReadtDtos
                    {
                        IdProducto = p.IdProducto,
                        Nombre = p.Nombre,
                        Precio = p.Precio,
                        Existencias = p.Existencias,
                        IdCategoria = p.IdCategoria

                    });
                    return Ok(result);
                } */

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int page = 1, int pageSize = 10, string? categoria = null, string? busqueda = null)
        {
            var (items, total) = await _productoService.GetPagedAsync(page, pageSize, categoria, busqueda);

            return Ok(new
            {
                Items = items,
                TotalCount = total,
                CurrentPage = page,
                PageSize = pageSize
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByI(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            return producto == null ? NotFound() : Ok(producto); 

        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductoCreateDtos dto)
        {
            try
            {
                var result = await _productoService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByI), new { id = result.IdProducto }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id:int}")]

        public async Task<IActionResult> Patch(int id, [FromBody] ProductoUpdateDto dto)
        {
            var update = await _productoService.UpdateAsync(id, dto);
            return update ? NoContent(): NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _productoService.DeleteAsync(id) ? NoContent() : NotFound();
        }

    }
}