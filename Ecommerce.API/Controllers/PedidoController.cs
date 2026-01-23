using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.DTOs;
using Ecommerce.API.Interfaces;
using Ecommerce.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
    private readonly IPedido _pedidoService;

    public PedidoController(IPedido pedidoService)
    {
        _pedidoService = pedidoService;
    } 

    [HttpGet("paged")]
public async Task<IActionResult> GetPaged([FromQuery]int page = 1,[FromQuery] int pageSize = 10)
{
    var (items, total) = await _pedidoService.GetPagedAsync(page, pageSize);


    return Ok(
        new
        {
            page,
            pageSize,
            total,
            totalPages = (int)Math.Ceiling((double)total / pageSize),
            date = items
        });
}


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
        {
            var pedido = await _pedidoService.GetByIdAsync(id);
            if(pedido == null) return NotFound();

            //Mapeo DTO detalles con nombres
            var result = new PedidoReadDto
            {
               IdPedido = pedido.IdPedido,
               FechaPedido = DateOnly.FromDateTime(pedido.FechaPedido),
               Total = pedido.Total,
               Estado = pedido.Estado,
               //Lista de los detalles mapeada
               Detalles = pedido.Detalles.Select(d => new DetallePedidoReadDto
               {
                   IdDetallePedido = d.IdDetallePedido,
                   IdProducto = d.IdProducto,
                   NombreProducto = d.Producto?.Nombre ?? "Producto no encontrado",
                   Cantidad = d.Cantidad,
                   PrecioUnitario = d.PrecioUnitario

               }) .ToList()
            };

            return Ok(result);
        }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PedidoCreateDto dto)
        {
            try
            {
                //Aqui se convierte un DTO a un Modelo
                var pedido = new Pedido
                {
                   IdCliente = dto.IdCliente,
                   IdMetodoPago = dto.IdMetodoPago,
                   Detalles = dto.Detalles.Select(d => new DetallePedido
                   {
                       IdProducto = d.IdProducto,
                       Cantidad = d.Cantidad
                   }).ToList()
                };
                
                var created = await _pedidoService.CreateAsync(pedido);
                return CreatedAtAction(
                    nameof(GetById),
                    new {id = created.IdPedido},
                    new {created.IdPedido}
                );
            } 
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PedidoUpdateDto dto) 
        {
           try
            {
              var update = await _pedidoService.UpdateAsync(id, dto);
              if(!update) return NotFound(new { message = "Pedido no encontrado"});

              return Ok(new {message = "Pedido actualizado"});  
            } catch (Exception ex)
            {
                return BadRequest(new {error = ex.Message});
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var delete = await _pedidoService.DeleteAsync(id);
                if(!delete) return NotFound(new {message = "Pedido no encontrado"});

                return Ok(new {message = "Pedido eliminado"});

            } catch(Exception ex)
            {
                return BadRequest(new {error = ex.Message});
            }
        }


    }
}
