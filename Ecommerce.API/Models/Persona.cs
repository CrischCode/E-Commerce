
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Models;

[Table("persona")]
public class Persona
{
    [Key]
    [Column("id_persona")]
    public Guid IdPersona {get; set;}

    [Column("primer_nombre")]
    [MaxLength(50)]
    public string PrimerNombre {get; set;} = null!;

    [Column("segundo_nombre")]
    [MaxLength(50)]
    public string? SegundoNombre {get; set;}

    [Column("primer_apellido")]
    [MaxLength(50)]
    public string PrimerApellido {get; set;} = null!;

    [Column("segundo_apellido")]
    [MaxLength(50)]
    public string? SegundoApellido {get; set;} 

    [Column("telefono")]
    [MaxLength(20)]
    public string? Telefono { get; set; }

    [Column("fecha_nacimiento", TypeName = "date")]
    public DateOnly? FechaNacimiento {get; set;}

     [Column("activo")]
    public bool Activo { get; set; } = true;

    [Column("fecha_registro")]
    public DateTime? FechaRegistro { get; set; } = DateTime.UtcNow;

    //Las relaciones 
     public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    // 🔹 1 Persona → MUCHOS Empleados
    public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();

    public ICollection<PersonaRol> PersonaRoles { get; set; } = new List<PersonaRol>();
}