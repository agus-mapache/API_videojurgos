using APIVideoJuegos.Features.Resena.DTOs;

namespace APIVideoJuegos.Features.Resena.Service;

public interface IResenaService
{
    Task<IEnumerable<ResenaResponseDto>> GetByJuegoAsync(int idJuego);
    Task<IEnumerable<ResenaResponseDto>> GetByUsuarioAsync(int idUsuario);
    Task<ResenaResponseDto?> GetByIdAsync(int id);
    Task<ResenaResponseDto> CreateAsync(CreateResenaDto dto);
    Task<bool> UpdateAsync(int id, UpdateResenaDto dto);
    Task<bool> DeleteAsync(int id);
}