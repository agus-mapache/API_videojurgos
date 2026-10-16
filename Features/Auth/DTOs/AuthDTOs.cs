namespace APIVideoJuegos.Features.Auth.DTOs;

using APIVideoJuegos.Features.Usuarios.DTOS;

public record LoginRequestDto(
    string Email, 
    string Password
);

public record LoginResponseDto(
    string Token, 
    UsuarioResponseDto Usuario
);

public record RegisterRequestDto(
    string NombreUsuario,
    string Email,
    string Password,
    string? TipoUsuario = "Cliente",
    int? IdEmpresa = null
);
