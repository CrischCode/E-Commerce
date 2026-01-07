using Ecommerce.API.Data;
using Ecommerce.API.Models;
using Ecommerce.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Service;

public class PersonaService : IPersonaService
{
    private readonly AppDbContext _context;
    
    public PersonaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Persona>> GetAllAsync()
    {
        return await _context.Persona
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<Persona?> GetByIdAsync(Guid id)
    {
        return await _context.Persona
        //.AsNoTracking()
        .FirstOrDefaultAsync(p => p.IdPersona == id);
    }

    public async Task<Persona> CreateAsync(Persona persona)
    {
        persona.IdPersona = Guid.NewGuid();
        persona.FechaRegistro = DateOnly.FromDateTime(DateTime.UtcNow);
        _context.Persona.Add(persona);
        await _context.SaveChangesAsync();
        return persona;
    }

  public async Task UpdateAsync(Persona persona)
    {
       // _context.Persona.Update(persona);
        await _context.SaveChangesAsync();
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        var persona = await _context.Persona.FindAsync(id);
        if(persona == null) return false;

        _context.Persona.Remove(persona);
        await _context.SaveChangesAsync();
        return true;
    }
}