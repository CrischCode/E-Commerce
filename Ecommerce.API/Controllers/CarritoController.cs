using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Shared.DTOs;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarritoController : ControllerBase
    {
    private readonly ICarrito _carritoService;

    public CarritoController(ICarrito carritoService)
    {
        _carritoService = carritoService;
    }

    [HttpGet("{idCliente}")]
    public async Task<IActionResult> Get(int idCliente)
        {
           try
            {
               var res = await _carritoService.GetCarritoByClienteAsync(idCliente);
        
            if (res == null) 
            return NotFound("Carrito vacío o no encontrado");

            return Ok(res);

            } catch (Exception ex)
            {
                return BadRequest($"Error al obtener el carrito: {ex.Message}");
            }
        }
    
    [HttpPost("agregar")]
    public async Task<IActionResult> Agregar([FromBody] AgregarProductoDto dto) 
        {
        var ok = await _carritoService.AgregarProductoAsync(dto.IdCliente, dto.IdProducto, dto.Cantidad);
        return ok ? Ok() : BadRequest("Error al agregar");
        }
    
    [HttpDelete("eliminar/{idCarrito}/{idProducto}")]
    public async Task<IActionResult> Eliminar(int idCarrito, int idProducto) {
        var ok = await _carritoService.EliminarProductoAsync(idCarrito, idProducto);
        return ok ? Ok() : NotFound();
    }

    [HttpPost("nuevo")]
        public async Task<IActionResult> Confirmar([FromBody] ConfirmarPedidoDto dto)
        {
            var resultado = await _carritoService.ProcesarCompraAsync(dto);
            if(resultado) return Ok("Pedido realizado con exito");
            return BadRequest("No se puedo completar el pedido");
        }
        
    }
}