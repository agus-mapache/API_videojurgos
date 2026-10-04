namespace APIVideoJuegos.Features.Usuarios.Repository;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<Usuario?> GetByIdAsync(int id);
    Task<int> CreateAsync(Usuario usuario);
    Task<bool> UpdateAsync(Usuario usuario);
    Task<bool> DeleteAsync(int id);
}