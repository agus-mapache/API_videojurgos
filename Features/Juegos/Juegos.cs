namespace APIVideoJuegos.Features.Juegos;

public class Juego
{
    public int IdJuego { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal PrecioBase { get; set; }
    public int DescuentoPorcentaje { get; set; }
    public DateTime? FechaLanzamiento { get; set; }
    
    // Relaciones (pueden ser nulas por el ON DELETE SET NULL)
    public int? IdDesarrollador { get; set; }
    public int? IdEditor { get; set; }
}