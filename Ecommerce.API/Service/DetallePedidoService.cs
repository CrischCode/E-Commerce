using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Interfaces;
using Ecommerce.API.Data;
using Ecommerce.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Service
{
    public class DetallePedidoService //: IDetallePedido
    {
    private readonly AppDbContext _context;
    
    public DetallePedidoService(AppDbContext context)
    {
        _context = context;
    }

/*
    public async Task<IEnumerable<DetallePedido>> GetAllAsync()
        {
             return await _context.DetallePedido
            .Include(d => d.Producto)
            .Where(d => d.IdPedido == IdPedido)
            .AsNoTracking()
            .ToListAsync();
            
        } */

    }
}