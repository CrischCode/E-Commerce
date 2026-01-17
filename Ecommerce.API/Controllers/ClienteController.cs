using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.API.DTOs;
using Ecommerce.API.Models;
using Ecommerce.API.Service;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
    private readonly ICliente _ClienteService;

    public ClienteController(ICliente clienteService)
    {
        _ClienteService = clienteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
           var result = await _ClienteService.GetPagedAsync(page, pageSize);
           return Ok(new {result.Total, result.Items}); 
        }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _ClienteService.GetByIdAsync(id);
            if(cliente == null) return NotFound();
            return Ok(cliente);
        }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClienteCreateDto dto)
        {
            var cliente = await _ClienteService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new {id = cliente.IdCliente}, cliente);
        }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDto dto)
        {
            var update = await _ClienteService.UpdateAsync(id, dto);
            if(!update) return NotFound();
            return NoContent();
        }
    
    }
}