using APIVideoJuegos.Features.Generos.DTOS;

namespace APIVideoJuegos.Features.Generos.Service;

public interface IGeneroService
{
    Task<IEnumerable<GeneroResponseDto>> GetAllAsync();
    Task<GeneroResponseDto?> GetByIdAsync(int id);
    Task<GeneroResponseDto> CreateAsync(CreateGeneroDto dto);
    Task<bool> UpdateAsync(int id, UpdateGeneroDto dto);
    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<GeneroResponseDto>> GetGenerosByJuegoAsync(int idJuego);
    Task<bool> AsignarGeneroAJuegoAsync(AsignarGeneroJuegoDto dto);
    Task<bool> RemoverGeneroDeJuegoAsync(int idJuego, int idGenero);
}