using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.API.Interfaces;
using Ecommerce.API.Data;
using Ecommerce.API.Models;
using Microsoft.EntityFrameworkCore;
using Ecommerce.API.DTOs;

namespace Ecommerce.API.Service;

public class PersonaService : IPersonaService
{
    private readonly AppDbContext _context;

    public PersonaService(AppDbContext context) => _context = context;

    public async Task<(IEnumerable<PersonaReadDto> Items, int Total)> GetPagedAsync(int page, int pageSize, Guid? idPersona, string? busqueda)
    {
        var query = _context.Persona.AsNoTracking();

         //filtro por persona
            if(idPersona.HasValue && idPersona.Value != Guid.Empty)
        {
           query = query.Where(p => p.IdPersona == idPersona.Value); 
        }

        if(!string.IsNullOrWhiteSpace(busqueda))
        {
            query = query.Where(p => p.PrimerNombre.Contains(busqueda) || p.PrimerApellido.Contains(busqueda));
        }
        //total de la bsuqueda
        var total = await query.CountAsync();
        
        var items = await query
            .OrderByDescending(p => p.FechaRegistro)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => MapToReadDto(p))
            .ToListAsync();

        return (items, total);
    }

    public async Task<PersonaReadDto?> GetByIdAsync(Guid id)
    {
        var persona = await _context.Persona.AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdPersona == id);
            
        return persona != null ? MapToReadDto(persona) : null;
    }

    public async Task<PersonaReadDto> CreateAsync(PersonaCreateDto dto)
    {
        var persona = new Persona
        {
            IdPersona = Guid.NewGuid(), 
            PrimerNombre = dto.PrimerNombre,
            SegundoNombre = dto.SegundoNombre,
            PrimerApellido = dto.PrimerApellido,
            SegundoApellido = dto.SegundoApellido,
            Telefono = dto.Telefono,
            FechaNacimiento = dto.FechaNacimiento,
            Activo = true
        };

        _context.Persona.Add(persona);
        await _context.SaveChangesAsync();
        return MapToReadDto(persona);
    }

    public async Task<bool> PatchAsync(Guid id, PersonaPatchDto dto)
    {
        var persona = await _context.Persona.FindAsync(id);
        if (persona == null) return false;

        if (!string.IsNullOrWhiteSpace(dto.PrimerNombre)) 
            persona.PrimerNombre = dto.PrimerNombre;
        if (!string.IsNullOrWhiteSpace(dto.SegundoNombre)) 
            persona.SegundoNombre = dto.SegundoNombre;
        if (!string.IsNullOrWhiteSpace(dto.PrimerApellido)) 
            persona.PrimerApellido = dto.PrimerApellido;
        if (!string.IsNullOrWhiteSpace(dto.SegundoApellido)) 
            persona.SegundoApellido = dto.SegundoApellido;
        if (!string.IsNullOrWhiteSpace(dto.Telefono)) 
            persona.Telefono = dto.Telefono;
        if (dto.FechaNacimiento.HasValue) 
            persona.FechaNacimiento = dto.FechaNacimiento.Value;

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var persona = await _context.Persona.FindAsync(id);
        if (persona == null) return false;

        _context.Persona.Remove(persona);
        return await _context.SaveChangesAsync() > 0;
    }

    //metodo de mapeo reutilizable
    private static PersonaReadDto MapToReadDto(Persona p) => new PersonaReadDto
    {
        IdPersona = p.IdPersona,
        PrimerNombre = p.PrimerNombre,
        SegundoNombre = p.SegundoNombre,
        PrimerApellido = p.PrimerApellido,
        SegundoApellido = p.SegundoApellido,
        Telefono = p.Telefono,
        FechaNacimiento = p.FechaNacimiento,
        Activo = p.Activo,
        FechaRegistro = p.FechaRegistro
    };
}