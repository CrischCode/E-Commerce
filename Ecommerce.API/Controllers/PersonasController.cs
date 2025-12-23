using Ecommerce.API.Models;
using Ecommerce.API.Service;
using Ecommerce.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class PersonaController : ControllerBase
{
    private readonly IPersonaService _personaService;

    public PersonaController(IPersonaService personaService)
    {
        _personaService = personaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllId()
    {
        var persona = await _personaService.GetAllAsync();
        return Ok(persona);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var persona = await _personaService.GetByIdAsync(id);
        if(persona == null)
        return NotFound();

        return Ok(persona);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Persona persona)
    {
        var created = await _personaService.CreateAsync(persona);
        return CreatedAtAction(nameof(GetById), new {id = created.IdPersona }, created);
    }
}