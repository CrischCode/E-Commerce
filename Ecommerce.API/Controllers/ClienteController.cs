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
    public class ClienteController : ControllerBase
    {
    private readonly ICliente _ClienteService;

    public ClienteController(ICliente clienteService)
    {
        _ClienteService = clienteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] int? idCliente = null, [FromQuery] string? busqueda = null)
        {
           var result = await _ClienteService.GetPagedAsync(page, pageSize, idCliente, busqueda);
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
    
    /*
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDto dto)
        {
            var update = await _ClienteService.UpdateAsync(id, dto);
            if(!update) return NotFound();
            return NoContent();
        } */

    [HttpPost("registro")]
    public async Task<IActionResult> Registrar([FromBody] RegistroDto dto)
        {
            try
            {
              await _ClienteService.RegistrarUsuarioCompletoAsync(dto);
              return Ok(new {message = "Usuario creado correctamente"});  
            } catch (Exception ex)
            {
                return BadRequest(new {error = ex.Message});
            }
        }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDto dto)
        {
          try
            {
                var clienteActulizado = await _ClienteService.UpdateAsync(id, dto);
                if(!clienteActulizado)
                return NotFound(new {message =$"No se encontro al cliente"});

                return Ok(new {message = "Datos actualizados"});

            } catch(Exception ex)
            {
                return BadRequest(new {error = ex.Message});
            }
        }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        {
            try {
            var delete = await _ClienteService.DeleteAsync(id);
            if(!delete)
            {
                return NotFound(new {message = $"No se encontro al cliente con ID {id}"});
            }
            return NoContent();
            } catch (Exception ex)
            {
                return BadRequest(new {error = "No se pudo desactivar al cliente: " + ex.Message});
            }

        }
    
    }
}