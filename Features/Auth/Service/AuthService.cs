namespace APIVideoJuegos.Features.Auth.Service;

using APIVideoJuegos.Features.Auth.DTOs;
using APIVideoJuegos.Features.Usuarios.DTOS;
using APIVideoJuegos.Features.Usuarios.Repository;

public class AuthService(IUsuarioRepository usuarioRepository, TokenService tokenService) : IAuthService
{
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var usuario = await usuarioRepository.GetByEmailAsync(dto.Email);
        if (usuario is null)
        {
            return null;
        }

        bool esValida = false;
        try
        {
            esValida = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);
        }
        catch
        {
            esValida = false;
        }

        if (!esValida)
        {
            return null;
        }

        var token = tokenService.GenerarToken(usuario);

        var usuarioDto = new UsuarioResponseDto(
            usuario.IdUsuario,
            usuario.NombreUsuario,
            usuario.Email,
            usuario.TipoUsuario,
            usuario.IdEmpresa,
            usuario.FechaRegistro
        );

        return new LoginResponseDto(token, usuarioDto);
    }
}
