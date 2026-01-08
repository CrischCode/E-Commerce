namespace Ecommerce.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Producto
{
    [Key]
    public int IdProducto {get; set;}
    [Required]
    [MaxLength(200)]
    public string? Nombre {get; set;}
    [Column(TypeName = "numeric(18,2)")]
    public decimal Precio {get; set;}
    public int Existencias {get; set;}
    public int IdCategoria {get; set;}
}