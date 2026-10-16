namespace APIVideoJuegos.Features.Auth.Service;

using APIVideoJuegos.Features.Auth.DTOs;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
}
