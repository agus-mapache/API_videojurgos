using APIVideoJuegos.Features.Usuarios.DTOS;
using APIVideoJuegos.Features.Usuarios.Repository;

namespace APIVideoJuegos.Features.Usuarios.service;

public class UsuarioService(IUsuarioRepository repository) : IUsuarioService
{
    public async Task<IEnumerable<UsuarioResponseDto>> GetAllAsync()
    {
        var usuarios = await repository.GetAllAsync();
        return usuarios.Select(MapToDto);
    }

    public async Task<UsuarioResponseDto?> GetByIdAsync(int id)
    {
        var usuario = await repository.GetByIdAsync(id);
        return usuario is null ? null : MapToDto(usuario);
    }

    public async Task<UsuarioResponseDto> CreateAsync(CreateUsuarioDto dto)
    {
        var nuevoUsuario = new Usuario
        {
            NombreUsuario = dto.NombreUsuario,
            Email = dto.Email,
            PasswordHash = dto.Password,
            TipoUsuario = string.IsNullOrWhiteSpace(dto.TipoUsuario) ? "Cliente" : dto.TipoUsuario,
            IdEmpresa = dto.IdEmpresa
        };

        var idGenerado = await repository.CreateAsync(nuevoUsuario);

        return new UsuarioResponseDto(
            idGenerado,
            nuevoUsuario.NombreUsuario,
            nuevoUsuario.Email,
            nuevoUsuario.TipoUsuario,
            nuevoUsuario.IdEmpresa,
            DateTime.UtcNow
        );
    }

    public async Task<bool> UpdateAsync(int id, UpdateUsuarioDto dto)
    {
        var usuarioExistente = await repository.GetByIdAsync(id);
        if (usuarioExistente is null) return false;

        usuarioExistente.NombreUsuario = dto.NombreUsuario;
        usuarioExistente.Email = dto.Email;
        usuarioExistente.TipoUsuario = string.IsNullOrWhiteSpace(dto.TipoUsuario) ? "Cliente" : dto.TipoUsuario;
        usuarioExistente.IdEmpresa = dto.IdEmpresa;

        return await repository.UpdateAsync(usuarioExistente);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }

    private static UsuarioResponseDto MapToDto(Usuario u) => new(
        u.IdUsuario,
        u.NombreUsuario,
        u.Email,
        u.TipoUsuario,
        u.IdEmpresa,
        u.FechaRegistro
    );
}