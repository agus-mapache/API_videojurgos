using APIVideoJuegos.Features.Juegos.DTOS;

namespace APIVideoJuegos.Features.Juegos.Service;

public interface IJuegoService
{
    Task<IEnumerable<JuegoResponseDto>> GetAllAsync();
    Task<JuegoResponseDto?> GetByIdAsync(int id);
    Task<JuegoResponseDto?> GetBySlugAsync(string slug);
    Task<JuegoResponseDto> CreateAsync(CreateJuegoDto dto);
    Task<bool> UpdateAsync(int id, UpdateJuegoDto dto);
    Task<bool> DeleteAsync(int id);
}