namespace APIVideoJuegos.Features.Juegos.Repository;

public interface IJuegoRepository
{
    Task<IEnumerable<Juego>> GetAllAsync();
    Task<Juego?> GetByIdAsync(int id);
    Task<Juego?> GetBySlugAsync(string slug); 
    Task<int> CreateAsync(Juego juego);
    Task<bool> UpdateAsync(Juego juego);
    Task<bool> DeleteAsync(int id);
}