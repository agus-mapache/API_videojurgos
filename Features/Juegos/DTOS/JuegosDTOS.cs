namespace APIVideoJuegos.Features.Juegos.DTOS;

public record CreateJuegoDto(
    string Titulo, 
    string Slug, 
    string? Descripcion, 
    decimal PrecioBase, 
    int DescuentoPorcentaje, 
    DateTime? FechaLanzamiento, 
    int? IdDesarrollador, 
    int? IdEditor
);

public record UpdateJuegoDto(
    string Titulo, 
    string Slug, 
    string? Descripcion, 
    decimal PrecioBase, 
    int DescuentoPorcentaje, 
    DateTime? FechaLanzamiento, 
    int? IdDesarrollador, 
    int? IdEditor
);

public record JuegoResponseDto(
    int IdJuego, 
    string Titulo, 
    string Slug, 
    string? Descripcion, 
    decimal PrecioBase, 
    int DescuentoPorcentaje, 
    DateTime? FechaLanzamiento, 
    int? IdDesarrollador, 
    int? IdEditor
);