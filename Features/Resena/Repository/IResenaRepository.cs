namespace APIVideoJuegos.Features.Resena.Repository;

public interface IResenaRepository
{
    Task<IEnumerable<Resena>> GetByJuegoIdAsync(int idJuego);
    Task<IEnumerable<Resena>> GetByUsuarioIdAsync(int idUsuario);
    Task<Resena?> GetByIdAsync(int id);
    Task<int> CreateAsync(Resena resena);
    Task<bool> UpdateAsync(Resena resena);
    Task<bool> DeleteAsync(int id);
}
