using APIVideoJuegos.Features.Usuarios.DTOS;

namespace APIVideoJuegos.Features.Usuarios.service;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioResponseDto>> GetAllAsync();
    Task<UsuarioResponseDto?> GetByIdAsync(int id);
    Task<UsuarioResponseDto> CreateAsync(CreateUsuarioDto dto);
    Task<bool> UpdateAsync(int id, UpdateUsuarioDto dto);
    Task<bool> DeleteAsync(int id);
}
