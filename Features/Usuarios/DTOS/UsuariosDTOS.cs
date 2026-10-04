namespace APIVideoJuegos.Features.Usuarios.DTOS;

public record CreateUsuarioDto(
    string NombreUsuario, 
    string Email, 
    string Password,
    string? TipoUsuario, // 'Cliente', 'Desarrollador', 'Admin'
    int? IdEmpresa       // Requerido si es desarrollador de una empresa
);

public record UpdateUsuarioDto(
    string NombreUsuario,
    string Email
)
{
    public int? IdEmpresa { get; internal set; }
    public string? TipoUsuario { get; internal set; }
}

public record UsuarioResponseDto(
    int IdUsuario, 
    string NombreUsuario, 
    string Email,
    string TipoUsuario,
    int? IdEmpresa,
    DateTime FechaRegistro
);