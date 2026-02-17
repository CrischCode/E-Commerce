using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Shared.DTOs;
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
public async Task<IActionResult> GetPaged([FromQuery]int page = 1,[FromQuery] int pageSize = 10, [FromQuery] string? estado = null, [FromQuery] int? idPedido = null)
{
    var (items, total) = await _pedidoService.GetPagedAsync(page, pageSize, estado, idPedido);


    return Ok(
        new
        {
            page,
            pageSize,
            total,
            totalPages = (int)Math.Ceiling((double)total / pageSize),
            date = items //debe de ser data pero me equivoque, no quiero mover nada jaja
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
               IdCliente = pedido.IdCliente,
               Cliente = pedido.Cliente?.Persona != null
               ? $"{pedido.Cliente.Persona.PrimerNombre} {pedido.Cliente.Persona.PrimerApellido}" : "Cliente no encontrado",
               IdMetodoPago = pedido.IdMetodoPago,
               MetodoPago = pedido.MetodoPago?.Nombre ?? "No especificado",
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
                   PrecioUnitario = d.PrecioUnitario,
                   SubTotal = d.SubTotal
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

                if (created == null) 
                    return BadRequest(new { message = "No se pudo crear el pedido" });
                
                var result = new PedidoReadDto
                {
                    IdPedido = created.IdPedido,
                    IdCliente = created.IdCliente,
                    Cliente = created.Cliente?.Persona != null
                    ? $"{created.Cliente.Persona.PrimerNombre} {created.Cliente.Persona.PrimerApellido}" : "Cliente no encontrado",
                    IdMetodoPago = created.IdMetodoPago,
                    MetodoPago = created.MetodoPago?.Nombre ?? "No especificado",
                    FechaPedido = DateOnly.FromDateTime(created.FechaPedido),
                    Total = created.Total,
                    Estado = created.Estado,
                    Detalles = created.Detalles.Select(d => new DetallePedidoReadDto
                    {
                        IdDetallePedido = d.IdDetallePedido,
                        IdProducto = d.IdProducto,
                        NombreProducto = d.Producto?.Nombre ?? "Producto",
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        SubTotal = d.SubTotal
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetById), new {id = result.IdPedido}, result);
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

                return Ok(new {message = "Pedido cancelado"});

            } catch(Exception ex)
            {
                return BadRequest(new {error = ex.Message});
            }
        }
    }
}
