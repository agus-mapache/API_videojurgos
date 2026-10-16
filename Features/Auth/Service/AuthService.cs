namespace APIVideoJuegos.Features.Auth.Service;

using APIVideoJuegos.Features.Auth.DTOs;
using APIVideoJuegos.Features.Usuarios;
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

    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NombreUsuario))
        {
            throw new ArgumentException("El nombre de usuario es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new ArgumentException("El email es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new ArgumentException("La contraseña es obligatoria.");
        }

        if (dto.Password.Length < 6)
        {
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");
        }

        // Validar si el email ya existe
        var existenteEmail = await usuarioRepository.GetByEmailAsync(dto.Email.Trim());
        if (existenteEmail is not null)
        {
            throw new InvalidOperationException("El correo electrónico ya se encuentra registrado.");
        }

        // Validar si el nombre de usuario ya existe
        var existenteNombre = await usuarioRepository.GetByNombreUsuarioAsync(dto.NombreUsuario.Trim());
        if (existenteNombre is not null)
        {
            throw new InvalidOperationException("El nombre de usuario ya se encuentra registrado.");
        }

        // Rol: Por seguridad pública en sign-up, solo 'Cliente' o 'Desarrollador'
        var tipoUsuario = "Cliente";
        if (string.Equals(dto.TipoUsuario, "Desarrollador", StringComparison.OrdinalIgnoreCase))
        {
            tipoUsuario = "Desarrollador";
        }

        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var nuevoUsuario = new Usuario
        {
            NombreUsuario = dto.NombreUsuario.Trim(),
            Email = dto.Email.Trim().ToLowerInvariant(),
            PasswordHash = hash,
            SaldoBilletera = 0.00m,
            TipoUsuario = tipoUsuario,
            IdEmpresa = tipoUsuario == "Desarrollador" ? dto.IdEmpresa : null,
            FechaRegistro = DateTime.UtcNow
        };

        var idGenerado = await usuarioRepository.CreateAsync(nuevoUsuario);
        nuevoUsuario.IdUsuario = idGenerado;

        var token = tokenService.GenerarToken(nuevoUsuario);

        var usuarioDto = new UsuarioResponseDto(
            nuevoUsuario.IdUsuario,
            nuevoUsuario.NombreUsuario,
            nuevoUsuario.Email,
            nuevoUsuario.TipoUsuario,
            nuevoUsuario.IdEmpresa,
            nuevoUsuario.FechaRegistro
        );

        return new LoginResponseDto(token, usuarioDto);
    }
}

