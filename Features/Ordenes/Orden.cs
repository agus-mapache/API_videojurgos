namespace APIVideoJuegos.Features.Ordenes;

public class Orden
{
    public int IdOrden { get; set; }
    public int IdUsuario { get; set; }
    public decimal MontoTotal { get; set; }
    public string Estado { get; set; } = "Pendiente"; // 'Pendiente', 'Completado', 'Reembolsado'
    public string? MetodoPago { get; set; }
    public DateTime FechaCreacion { get; set; }
    
    public List<DetalleOrden> Detalles { get; set; } = [];
}

public class DetalleOrden
{
    public int IdDetalle { get; set; }
    public int IdOrden { get; set; }
    public int IdJuego { get; set; }
    public decimal PrecioComprado { get; set; }
}