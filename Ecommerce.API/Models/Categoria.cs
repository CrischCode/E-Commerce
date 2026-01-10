using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.API.Models;

[Table("categoria")]
public class Categoria
{
    [Key]
    [Column("id_categoria")]
    public int Id_Categoria {get; set;}

    [Required]
    [MaxLength(100)]
    [Column("nombre")]
    public string? Nombre {get; set;}
    [Column("descripcion")]
    public string Descripcion {get; set;} = null!;
    
}