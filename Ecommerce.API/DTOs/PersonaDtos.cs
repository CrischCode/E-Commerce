using System.ComponentModel.DataAnnotations;

namespace Ecommerce.API.DTOs
{
    public class PersonaCreateDto
    {
    public string PrimerNombre { get; set; } = null!;
    public string? SegundoNombre { get; set; }
    public string PrimerApellido { get; set; } = null!;
    public string? SegundoApellido { get; set; }
    public string? Telefono { get; set; }
    public DateOnly? FechaNacimiento { get; set; }
    }

    public class PersonaUpdateDto
    {
    public string PrimerNombre { get; set; } = null!;
    public string? SegundoNombre { get; set; }
    public string PrimerApellido { get; set; } = null!;
    public string? SegundoApellido { get; set; }
    public string? Telefono { get; set; }
    public DateOnly? FechaNacimiento { get; set; }
    }

    public class PersonaReadDto
    {
    public Guid IdPersona { get; set; }
    public string PrimerNombre { get; set; } = null!;
    public string? SegundoNombre { get; set; }
    public string PrimerApellido { get; set; } = null!;
    public string? SegundoApellido { get; set; }
    public string? Telefono { get; set; }
    public DateOnly? FechaNacimiento { get; set; }
    public bool Activo { get; set; }
    public DateTime? FechaRegistro { get; set; }
    }

    public class PersonaPatchDto
    {
    public string? PrimerNombre { get; set; }
    public string? SegundoNombre { get; set; }
    public string? PrimerApellido { get; set; }
    public string? SegundoApellido { get; set; }
    public string? Telefono { get; set; }
    public DateOnly? FechaNacimiento { get; set; }
    }
}