namespace APIVideoJuegos.Features.Resena.DTOs;

public record CreateResenaDto(
    int IdUsuario,
    int IdJuego,
    bool Recomendado,
    string? Comentario
);

public record UpdateResenaDto(
    bool Recomendado,
    string? Comentario
);

public record ResenaResponseDto(
    int IdResena,
    int IdUsuario,
    int IdJuego,
    bool Recomendado,
    string? Comentario,
    DateTime FechaPublicacion
);