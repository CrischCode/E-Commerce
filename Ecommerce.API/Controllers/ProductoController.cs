using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ecommerce.API.DTOs;
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
        public async Task<IActionResult> GetPaged(int page = 1, int pageSize = 10, string? categoria = null)
        {
            var (items, total) = await _productoService.GetPagedAsync(page, pageSize, categoria);
    
    return Ok(new {
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
            if (producto == null) return NotFound();

            return Ok(new ProductoReadtDtos
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Existencias = producto.Existencias,
                IdCategoria = producto.IdCategoria
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductoCreateteDtos dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                Existencias = dto.Existencias,
                IdCategoria = dto.IdCategoria,
            };
            try
            {
                var create = await _productoService.CreateAsync(producto);
            return CreatedAtAction(nameof(GetByI), new {id = create.IdProducto}, create);
            }
            catch (KeyNotFoundException ex)
            {
               return BadRequest (new { message = ex.Message}); 
            }
        }

        [HttpPatch("{id:int}")]

        public async Task<IActionResult> Patch(int id, [FromBody] ProductoUpdateDto dto)
        {
            var producto = await _productoService.GetByIdAsync(id);
            if(producto == null) return NotFound();

            if(!string.IsNullOrWhiteSpace(dto.Nombre))
            producto.Nombre = dto.Nombre;

            if(dto.Precio.HasValue)
            producto.Precio = dto.Precio.Value;

            if(dto.Existencias.HasValue)
            producto.Existencias = dto.Existencias.Value;

            if(dto.IdCategoria.HasValue)
            {
                var categoriaExist = await _productoService.CategoriaExistAsync(dto.IdCategoria.Value);
                if(!categoriaExist) return BadRequest("La categoria no existe");
                producto.IdCategoria = dto.IdCategoria.Value;
            }

            await _productoService.UpdateAsync(producto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var delete = await _productoService.DeleteAsync(id);
            if(!delete) return NotFound();

            return NoContent();
        }
 
    }
}