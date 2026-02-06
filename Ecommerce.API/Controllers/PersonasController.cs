using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Ecommerce.API.DTOs;
using Ecommerce.Shared.DTOs;
using Ecommerce.API.Interfaces;
using Ecommerce.API.Models;
using Microsoft.AspNetCore.Mvc;


namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonaController : ControllerBase
{
    private readonly IPersonaService _service;

    public PersonaController(IPersonaService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] Guid? idPersona = null, [FromQuery] string? busqueda = null)
    {
        var (items, total) = await _service.GetPagedAsync(page, pageSize, idPersona, busqueda);
        return Ok(new {
            total,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling((double)total / pageSize),
            data = items
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var persona = await _service.GetByIdAsync(id);
        return persona != null ? Ok(persona) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonaCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.IdPersona }, result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Patch(Guid id, PersonaPatchDto dto)
    {
        var success = await _service.PatchAsync(id, dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}