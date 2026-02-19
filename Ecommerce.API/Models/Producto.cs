namespace Ecommerce.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("producto")]
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
    [Column("foto_data")]
    public byte[]? FotoData {get; set;}
    [Column("foto_mimetype")]
    public string? FotoMimeType {get; set;}

    [ForeignKey("IdCategoria")]
    public virtual Categoria? Categoria {get; set;}
}