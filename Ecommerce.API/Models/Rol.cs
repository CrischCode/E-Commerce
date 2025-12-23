using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.API.Models;

[Table("rol")]

public class Rol
{
    [Key]
    [Column("id_rol")]
    public int IdRol {get; set;}

    [Column("nombre")]
    [MaxLength(50)]
    public string Nombre {get; set;} = null!;

    public ICollection<PersonaRol> PersonaRoles { get; set; } = new List<PersonaRol>();
}