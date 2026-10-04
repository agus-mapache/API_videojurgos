namespace APIVideoJuegos.Features.Generos.Repository;

public interface IGeneroRepository
{
    Task<IEnumerable<Genero>> GetAllAsync();
    Task<Genero?> GetByIdAsync(int id);
    Task<int> CreateAsync(Genero genero);
    Task<bool> UpdateAsync(Genero genero);
    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<Genero>> GetGenerosByJuegoIdAsync(int idJuego);
    Task<bool> AsignarGeneroAJuegoAsync(int idJuego, int idGenero);
    Task<bool> RemoverGeneroDeJuegoAsync(int idJuego, int idGenero);
}