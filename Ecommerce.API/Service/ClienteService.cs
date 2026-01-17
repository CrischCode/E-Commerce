using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Interfaces;
using Ecommerce.API.Data;
using Ecommerce.API.Models;
using Microsoft.EntityFrameworkCore;
using Ecommerce.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Service
{
    public class ClienteService : ICliente
    {
        private readonly AppDbContext _context;

        public ClienteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<ClienteReadDto> Items, int Total)> GetPagedAsync(int page, int pageSize)
        {
            var query =
                from c in _context.Cliente
                join p in _context.Persona
                    on c.IdPersona equals p.IdPersona
                select new ClienteReadDto
                {
                    IdCliente = c.IdCliente,
                    IdPersona = p.IdPersona,
                    PrimerNombre = p.PrimerNombre,
                    SegundoNombre = p.SegundoNombre,
                    PrimerApellido = p.PrimerApellido,
                    SegundoApellido = p.SegundoApellido,
                    Telefono = p.Telefono,
                    FechaNacimiento = p.FechaNacimiento,
                    Puntos = c.Puntos,
                    FechaAlta = c.FechaAlta ?? DateTime.UtcNow
                };

            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<ClienteReadDto?> GetByIdAsync(int id)
        {
            return await _context.Cliente
            .Include(c => c.Persona)
            .Where(c => c.IdCliente == id)
            .Select(c => new ClienteReadDto
            {
                IdCliente = c.IdCliente,
                IdPersona = c.IdPersona,
                PrimerNombre = c.Persona.PrimerNombre,
                SegundoNombre = c.Persona.SegundoNombre,
                PrimerApellido = c.Persona.PrimerApellido,
                SegundoApellido = c.Persona.SegundoApellido,
                Telefono = c.Persona.Telefono,
                Puntos = c.Puntos
                
            })
            .FirstOrDefaultAsync();
        }

        public async Task<Cliente> CreateAsync(ClienteCreateDto dto)
{
    using var tx = await _context.Database.BeginTransactionAsync();
    try {
        var persona = await _context.Persona.FindAsync(dto.IdPersona);

        if (persona == null) {
            persona = new Persona {
                IdPersona = dto.IdPersona,
            };
            _context.Persona.Add(persona);
            await _context.SaveChangesAsync();
        }

        var cliente = new Cliente {
            IdPersona = persona.IdPersona,
            Puntos = 0,
            FechaAlta = DateTime.UtcNow
        };

        _context.Cliente.Add(cliente);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return cliente;
    } catch {
        await tx.RollbackAsync();
        throw;
    }
}

        public async Task<bool> UpdateAsync(int id, ClienteUpdateDto dto)
        {
            var cliente = await _context.Cliente
            .Include(c => c.Persona)
            .FirstOrDefaultAsync(c => c.IdCliente == id); 

            if(cliente == null) return false;

            cliente.Persona.PrimerNombre = dto.PrimerNombre;
            cliente.Persona.SegundoNombre = dto.SegundoNombre;
            cliente.Persona.PrimerApellido = dto.PrimerApellido;
            cliente.Persona.SegundoApellido = dto.SegundoApellido;
            cliente.Persona.Telefono = dto.Telefono;
            cliente.Persona.FechaNacimiento = dto.FechaNacimiento;

            await _context.SaveChangesAsync();
            return true;
        }
    public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if(cliente == null) return false;

            _context.Cliente.Remove(cliente);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}