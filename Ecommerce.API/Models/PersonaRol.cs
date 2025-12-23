using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.API.Models
{
    [Table("persona_rol")]
    public class PersonaRol
    {
    [Column("id_persona")]
    public Guid IdPersona { get; set; }

    [Column("id_rol")]
    public int IdRol { get; set; }

    public Persona Persona { get; set; } = null!;
    public Rol Rol { get; set; } = null!;
    }
}