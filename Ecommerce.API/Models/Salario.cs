using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.API.Models
{
    [Table("salario")]
 public class Salario
    {
    [Key]
    [Column("id_salario")]
    public int IdSalario { get; set; }

    [Column("id_empleado")]
    public int IdEmpleado { get; set; }

    [Column("monto")]
    public decimal Monto { get; set; }

    [Column("fecha_inicio")]
    public DateTime FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateTime? FechaFin { get; set; }

    public Empleado Empleado { get; set; } = null!;
    }
}