namespace APIVideoJuegos.Features.Generos.DTOS;

public record CreateGeneroDto(string Nombre);
public record UpdateGeneroDto(string Nombre);
public record GeneroResponseDto(int IdGenero, string Nombre);

// DTO para asociar un género a un juego mediante la tabla intermedia
public record AsignarGeneroJuegoDto(int IdJuego, int IdGenero);