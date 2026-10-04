namespace APIVideoJuegos.Features.Bibloteca;

public class BibliotecaItem
{
    public int IdUsuario { get; set; }
    public int IdJuego { get; set; }
    public DateTime FechaAdquisicion { get; set; }
    public decimal HorasJugadas { get; set; }
}