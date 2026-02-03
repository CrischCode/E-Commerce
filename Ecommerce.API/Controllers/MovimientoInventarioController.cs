using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.API.Interfaces;
using Ecommerce.API.DTOs;
using Ecommerce.API.Models;
using Ecommerce.API.Service;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientoInventarioController : ControllerBase
    {
       private readonly IMovimientoInventario _movimientoService;

    public MovimientoInventarioController(IMovimientoInventario movimientoService)
    {
        _movimientoService = movimientoService;
    } 

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery]int page = 1,[FromQuery] int pageSize = 10)
        {
            var (items, total) = await _movimientoService.GetPagedAsync(page, pageSize);


    return Ok(
        new
        {
            page,
            pageSize,
            total,
            totalPages = (int)Math.Ceiling((double)total / pageSize),
            data = items
        });
        }

    [HttpPost("ajustes")]
    public async Task<IActionResult> AjusteManual([FromBody] MovimientoCreateDto dto)
        {
            var result = await _movimientoService.CreateMovimientoAsync(dto);
            if(!result) return BadRequest("No se pudo realizar el ajuste");
            return Ok(new {message = "Ajuste realizado"});
        }
    }
}