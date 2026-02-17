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
    public class CatalogoClienteController : ControllerBase
    {
    private readonly ICatalogoCliente _CatalogoService;

    public CatalogoClienteController(ICatalogoCliente catalogoService)
    {
        _CatalogoService = catalogoService;
    }

    [HttpGet]
    public async Task<ActionResult<ProductoCatalogoResponse<ProductoReadClienteDto>>> Get([FromQuery] int page = 1,
    [FromQuery] int pageSize = 10, string? busqueda = null, string? categoria = null
    )
        {
            var (items, total) = await _CatalogoService.GetPagedAsync(page,pageSize, busqueda, categoria);

            var response = new ProductoCatalogoResponse<ProductoReadClienteDto>
            {
                Total = total,
                Data = items.ToList()
            };

            return Ok(response);
        }
    }
}