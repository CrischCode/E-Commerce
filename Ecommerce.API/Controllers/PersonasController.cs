using Ecommerce.API.Models;
using Ecommerce.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.API.DTOs;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonaController : ControllerBase
{
    private readonly IPersonaService _personaService;

    public PersonaController(IPersonaService personaService)
    {
        _personaService = personaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var personas = await _personaService.GetAllAsync();

        var result = personas.Select(p => new PersonaReadDto
        {
            IdPersona = p.IdPersona,
            PrimerNombre = p.PrimerNombre,
            SegundoNombre = p.SegundoNombre,
            PrimerApellido = p.PrimerApellido,
            SegundoApellido = p.SegundoApellido,
            Telefono = p.Telefono,
            FechaNacimiento = p.FechaNacimiento,
            Activo = p.Activo,
            FechaRegistro = p.FechaRegistro
        });
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var persona = await _personaService.GetByIdAsync(id);
        if (persona == null) return NotFound();

        return Ok(new PersonaReadDto
        {
            IdPersona = persona.IdPersona,
            PrimerNombre = persona.PrimerNombre,
            SegundoNombre = persona.SegundoNombre,
            PrimerApellido = persona.PrimerApellido,
            SegundoApellido = persona.SegundoApellido,
            Telefono = persona.Telefono,
            FechaNacimiento = persona.FechaNacimiento,
            Activo = persona.Activo,
            FechaRegistro = persona.FechaRegistro,
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonaCreateDto dto)
    {
        var persona = new Persona
        {
        PrimerNombre = dto.PrimerNombre,
        SegundoNombre = dto.SegundoNombre,
        PrimerApellido = dto.PrimerApellido,
        SegundoApellido = dto.SegundoApellido,
        Telefono = dto.Telefono,
        FechaNacimiento = dto.FechaNacimiento,
        };
        var created = await _personaService.CreateAsync(persona);
        return CreatedAtAction(nameof(GetById), new { id = created.IdPersona }, created);
    }
/*
[HttpPut("{id:guid}")]
public async Task<IActionResult> Update(Guid id, [FromBody] PersonaUpdateDto dto)
{
    var persona = await _personaService.GetByIdAsync(id);
    if (persona == null)
        return NotFound();

        //Mapeo DTO → entidad
        persona.PrimerNombre = dto.PrimerNombre;
        persona.SegundoNombre = dto.SegundoNombre;
        persona.PrimerApellido = dto.PrimerApellido;
        persona.SegundoApellido = dto.SegundoApellido;
        persona.Telefono = dto.Telefono;
        persona.FechaNacimiento = dto.FechaNacimiento;




    await _personaService.UpdateAsync(persona);

    return NoContent();
} */

[HttpPatch("{id}")]
public async Task<IActionResult> Patch(Guid id,[FromBody] PersonaPatchDto dto)
    {
        var persona = await _personaService.GetByIdAsync(id);
        if(persona == null)
        return NotFound();

    if (!string.IsNullOrWhiteSpace(dto.PrimerNombre))
        persona.PrimerNombre = dto.PrimerNombre;

    if (!string.IsNullOrWhiteSpace(dto.SegundoNombre))
        persona.SegundoNombre = dto.SegundoNombre;

    if (!string.IsNullOrWhiteSpace(dto.PrimerApellido))
        persona.PrimerApellido = dto.PrimerApellido;

    if (!string.IsNullOrWhiteSpace(dto.SegundoApellido))
        persona.SegundoApellido = dto.SegundoApellido;

    if (!string.IsNullOrWhiteSpace(dto.Telefono))
        persona.Telefono = dto.Telefono;

    if (dto.FechaNacimiento.HasValue)
        persona.FechaNacimiento = dto.FechaNacimiento.Value;

    await _personaService.UpdateAsync(persona);
    return NoContent(); 
    }



    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _personaService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
