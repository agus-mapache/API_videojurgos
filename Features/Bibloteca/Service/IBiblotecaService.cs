using APIVideoJuegos.Features.Bibloteca.DTOs;

namespace APIVideoJuegos.Features.Bibloteca.Service;

public interface IBibliotecaService
{
    Task<IEnumerable<BibliotecaResponseDto>> GetByUsuarioAsync(int idUsuario);
    Task<BibliotecaResponseDto?> GetJuegoEnBibliotecaAsync(int idUsuario, int idJuego);
    Task<bool> AddJuegoAsync(AddJuegoBibliotecaDto dto);
    Task<bool> UpdateHorasAsync(int idUsuario, int idJuego, UpdateHorasDto dto);
    Task<bool> RemoveJuegoAsync(int idUsuario, int idJuego);
}