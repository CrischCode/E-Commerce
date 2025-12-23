using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.API.Models;

[Table("cliente")]
public class Cliente
{
    
    [Key]
    [Column("id_cliente")]
    public int IdCliente {get; set;}

    [Column("id_persona")]
    public Guid IdPersona {get; set;}

    [Column("fecha_alta")]
    public DateTime? FechaAlta {get; set;} = DateTime.UtcNow;

    [Column("puntos")]
    public int Puntos {get; set;}

    public Persona Persona {get; set;} = null!;
    
}