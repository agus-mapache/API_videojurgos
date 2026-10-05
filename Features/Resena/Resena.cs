namespace APIVideoJuegos.Features.Resena;

public class Resena
{
    public int IdResena { get; set; }
    public int IdUsuario { get; set; }
    public int IdJuego { get; set; }
    public bool Recomendado { get; set; }
    public string? Comentario { get; set; }
    public DateTime FechaPublicacion { get; set; }
}
