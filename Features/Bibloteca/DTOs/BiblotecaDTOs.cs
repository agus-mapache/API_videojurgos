namespace APIVideoJuegos.Features.Bibloteca.DTOs;

public record AddJuegoBibliotecaDto(
    int IdUsuario, 
    int IdJuego
);

public record UpdateHorasDto(
    decimal HorasJugadas
);

public record BibliotecaResponseDto(
    int IdUsuario,
    int IdJuego,
    DateTime FechaAdquisicion,
    decimal HorasJugadas
);