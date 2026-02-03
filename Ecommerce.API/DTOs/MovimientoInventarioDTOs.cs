using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.API.DTOs
{
    public class MovimientoReadDto
{
    public int IdMovimiento { get; set; }
    public string? NombreProducto { get; set; } = null!;
    public string TipoMovimiento { get; set; } = null!; // Entrada/Salida
    public int Cantidad { get; set; }
    public string? Motivo { get; set; }
    public DateTime FechaMovimiento { get; set; }
}

        public class MovimientoCreateDto
{
    public int IdProducto { get; set; }
    public string TipoMovimiento { get; set; } = null!; // Entrada/Salida
    public int Cantidad { get; set; }
    public string? Motivo { get; set; }
    public DateTime FechaMovimiento { get; set; }
}
}