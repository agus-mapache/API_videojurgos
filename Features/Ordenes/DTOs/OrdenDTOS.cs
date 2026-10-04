namespace APIVideoJuegos.Features.Ordenes.DTOs;
public record CreateDetalleOrdenDto(
    int IdJuego, 
    decimal PrecioComprado
);

public record CreateOrdenDto(
    int IdUsuario, 
    string? MetodoPago,
    List<CreateDetalleOrdenDto> Detalles
);

public record DetalleOrdenResponseDto(
    int IdDetalle,
    int IdJuego,
    decimal PrecioComprado
);

public record OrdenResponseDto(
    int IdOrden,
    int IdUsuario,
    decimal MontoTotal,
    string Estado,
    string? MetodoPago,
    DateTime FechaCreacion,
    List<DetalleOrdenResponseDto> Detalles
);