namespace APIVideoJuegos.Features.Ordenes.Repository;

public interface IOrdenRepository
{
    Task<IEnumerable<Orden>> GetAllAsync();
    Task<Orden?> GetByIdAsync(int id);
    Task<IEnumerable<Orden>> GetByUsuarioIdAsync(int idUsuario);
    Task<int> CreateAsync(Orden orden, IEnumerable<DetalleOrden> detalles);
    Task<bool> UpdateEstadoAsync(int idOrden, string nuevoEstado);
    Task<bool> DeleteAsync(int id);
}