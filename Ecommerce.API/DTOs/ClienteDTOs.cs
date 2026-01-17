using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.API.DTOs
{
    public class ClienteReadDto
    {
        public int IdCliente { get; set; }
        public Guid IdPersona { get; set; }

        public string? PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }

        public string? Telefono { get; set; }
        public DateOnly? FechaNacimiento { get; set; }

        public int Puntos { get; set; }
        public DateTime FechaAlta { get; set; }

    }

    public class ClienteCreateDto
    {
        public Guid IdPersona { get; set; }

        public int Puntos {get; set;}

        public DateTime FechaAlta {get; set;}

    }
}

public class ClienteUpdateDto
{
    public string PrimerNombre { get; set; } = null!;
    public string? SegundoNombre { get; set; }
    public string PrimerApellido { get; set; } = null!;
    public string? SegundoApellido { get; set; }

    public string? Telefono { get; set; }
    public DateOnly? FechaNacimiento { get; set; }
}

