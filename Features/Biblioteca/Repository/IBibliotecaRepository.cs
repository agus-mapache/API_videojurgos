namespace APIVideoJuegos.Features.Biblioteca.Repository;

public interface IBibliotecaRepository
{
    Task<IEnumerable<BibliotecaItem>> GetByUsuarioIdAsync(int idUsuario);
    Task<BibliotecaItem?> GetAsync(int idUsuario, int idJuego);
    Task<bool> AddAsync(BibliotecaItem item);
    Task<bool> UpdateHorasAsync(int idUsuario, int idJuego, decimal horasJugadas);
    Task<bool> RemoveAsync(int idUsuario, int idJuego);
}
