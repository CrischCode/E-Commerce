using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Ecommerce.API.Models;

[Table("empleado")]

public class Empleado
{
    [Key]
    [Column("id_empleado")]
    public int IdEmpleado {get; set;}

    [Column("id_persona")]
    public Guid IdPersona {get; set;}

    [Column("fecha_ingreso")]
    public DateTime FechaIngreso {get; set;}

    [Column("activo")]
    public bool Activo {get; set;} = true;

    public Persona Persona { get; set; } = null!;
    public ICollection<Salario> Salarios { get; set; } = new List<Salario>();
}