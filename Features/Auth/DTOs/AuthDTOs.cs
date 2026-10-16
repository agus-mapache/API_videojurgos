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
