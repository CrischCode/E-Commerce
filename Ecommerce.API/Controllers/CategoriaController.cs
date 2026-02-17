using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
//using Ecommerce.API.DTOs;
using Ecommerce.Shared.DTOs;
using Ecommerce.API.Models;
using Ecommerce.API.Service;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
    private readonly ICategoria _categoriaService;

    public CategoriaController(ICategoria categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]

    public async Task<IActionResult> GetAll()
        {
            var categoria = await _categoriaService.GetAllAsync(); 
            var result = categoria.Select(c => new CategoriaReadDto
            {
                Id_Categoria = c.Id_Categoria,
                Nombre = c.Nombre!,
                Descripcion = c.Descripcion,
                
            });
            return Ok(result);
        }

    [HttpGet("{id:int}")]

    public async Task<IActionResult> GetById(int id)
        {
            var categoria = await _categoriaService.GetByIdAsync(id);
            if(categoria == null) return NotFound();

            var dto = new CategoriaReadDto
            {
                Id_Categoria = categoria.Id_Categoria,
                Nombre = categoria.Nombre!,
                Descripcion = categoria.Descripcion
            };
            return Ok(dto);
            
        }

        [HttpPost]
    public async Task<IActionResult> Create(CategoriaCreateDto dto)
        {
            var categoria = new Categoria
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };
            var created = await _categoriaService.CreateAsync(categoria);
            return CreatedAtAction(nameof(GetById), new { id = created.Id_Categoria }, created);
            
        }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] CategoriaUpdateDto dto)
        {
            var categoria = await _categoriaService.GetByIdAsync(id);
            if(categoria == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Nombre))
            categoria.Nombre = dto.Nombre;

            if(!string.IsNullOrWhiteSpace(dto.Descripcion))
            categoria.Descripcion = dto.Descripcion;

            await _categoriaService.UpdateAsync(categoria);
            return NoContent();
        }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoriaService.DeleteAsync(id);
            if(!deleted) return NotFound();

            return NoContent();
        }
        
    }
}