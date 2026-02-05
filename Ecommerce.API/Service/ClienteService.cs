using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Interfaces;
using Ecommerce.API.Data;
using Ecommerce.API.Models;
using Microsoft.EntityFrameworkCore;
//using Ecommerce.API.DTOs;
using Ecommerce.Shared.DTOs;
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

        public async Task<(IEnumerable<ClienteReadDto> Items, int Total)> GetPagedAsync(int page, int pageSize, int? idCliente, string? busqueda)
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
             //filtro por IdCliente
            if(idCliente.HasValue)
        {
           query = query.Where(p => p.IdCliente == idCliente.Value); 
        }

        //filtro por nombres y apellidos
        if(!string.IsNullOrWhiteSpace(busqueda))
        {
            string term = busqueda.ToLower().Trim();
            query = query.Where(c => c.PrimerNombre!.ToLower().Contains(term) || 
            c.PrimerApellido!.ToLower().Contains(term) || 
            (c.SegundoApellido != null && c.SegundoApellido.ToLower().Contains(term)));
        }

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
                PrimerNombre = c.Persona!.PrimerNombre,
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


            var personaExist = await _context.Persona.AnyAsync(p => p.IdPersona == dto.IdPersona);
            if (!personaExist)
            {
                throw new Exception("La persona no existe");
            }
            var clienteExist = await _context.Cliente.AnyAsync(c => c.IdPersona == dto.IdPersona);
            if (clienteExist)
            {
                throw new Exception("El cliente ya existe");
            }
            var persona = await _context.Persona.FindAsync(dto.IdPersona);

            if (persona == null)
            {
                persona = new Persona
                {
                    IdPersona = dto.IdPersona,
                };
                _context.Persona.Add(persona);
                await _context.SaveChangesAsync();
            }

            var cliente = new Cliente
            {
                IdPersona = persona.IdPersona,
                Puntos = 0,
                FechaAlta = DateTime.UtcNow
            };

            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return cliente;
        }

        public async Task<bool> UpdateAsync(int id, ClienteUpdateDto dto)
        {
            var cliente = await _context.Cliente
            .Include(c => c.Persona)
            .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.PrimerNombre))
                cliente.Persona.PrimerNombre = dto.PrimerNombre;

            if (!string.IsNullOrWhiteSpace(dto.SegundoNombre))
                cliente.Persona.SegundoNombre = dto.SegundoNombre;

            if (!string.IsNullOrWhiteSpace(dto.PrimerApellido))
                cliente.Persona.PrimerApellido = dto.PrimerApellido;

            if (!string.IsNullOrWhiteSpace(dto.SegundoApellido))
                cliente.Persona.SegundoApellido = dto.SegundoApellido;

            if (!string.IsNullOrWhiteSpace(dto.Telefono))
                cliente.Persona.Telefono = dto.Telefono;

            if (dto.FechaNacimiento.HasValue)
                cliente.Persona.FechaNacimiento = dto.FechaNacimiento;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RegistrarUsuarioCompletoAsync(RegistroDto dto)
        {
            using var tx = await _context.Database.BeginTransactionAsync();
            var existe = await _context.Persona.AnyAsync(p => p.IdPersona == dto.IdPersona);
            if (existe)
            {
                throw new Exception("Este usuario ya existe");
            }
            try
            {

                var nuevaPersona = new Persona
                {
                    IdPersona = dto.IdPersona,
                    PrimerNombre = dto.PrimerNombre,
                    SegundoNombre = dto.SegundoNombre,
                    PrimerApellido = dto.PrimerApellido,
                    SegundoApellido = dto.PrimerApellido,
                    Telefono = dto.Telefono,
                    FechaNacimiento = dto.FechaNacimiento,
                    Activo = true,
                    FechaRegistro = DateTime.UtcNow
                };
                _context.Persona.Add(nuevaPersona);

                var NuevoCliente = new Cliente
                {
                    IdPersona = dto.IdPersona,
                    Puntos = 0,
                    FechaAlta = DateTime.UtcNow
                };
                _context.Cliente.Add(NuevoCliente);

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
                return true;

            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                throw new Exception("Error al registrar:" + ex.Message);
            }
        }

          public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _context.Cliente
            .Include(c => c.Persona)
            .FirstOrDefaultAsync(c => c.IdCliente == id);

            if(cliente == null) return false;
            cliente.Persona.Activo = false; //Aqui se desactiva la persona en vez de borrarla

            await _context.SaveChangesAsync();
            return true;
        }
    }
}