using APIVideoJuegos.Features.Ordenes.DTOs;
using APIVideoJuegos.Features.Ordenes.Repository;

namespace APIVideoJuegos.Features.Ordenes.Service;

public class OrdenService(IOrdenRepository repository) : IOrdenService
{
    public async Task<IEnumerable<OrdenResponseDto>> GetAllAsync()
    {
        var ordenes = await repository.GetAllAsync();
        return ordenes.Select(MapToDto);
    }

    public async Task<OrdenResponseDto?> GetByIdAsync(int id)
    {
        var orden = await repository.GetByIdAsync(id);
        return orden is null ? null : MapToDto(orden);
    }

    public async Task<IEnumerable<OrdenResponseDto>> GetByUsuarioAsync(int idUsuario)
    {
        var ordenes = await repository.GetByUsuarioIdAsync(idUsuario);
        return ordenes.Select(MapToDto);
    }

    public async Task<OrdenResponseDto> CreateAsync(CreateOrdenDto dto)
    {
        // Calculamos el monto total sumando los precios de los juegos comprados
        var montoTotal = dto.Detalles.Sum(d => d.PrecioComprado);

        var nuevaOrden = new Orden
        {
            IdUsuario = dto.IdUsuario,
            MontoTotal = montoTotal,
            Estado = "Pendiente",
            MetodoPago = dto.MetodoPago
        };

        var listaDetalles = dto.Detalles.Select(d => new DetalleOrden
        {
            IdJuego = d.IdJuego,
            PrecioComprado = d.PrecioComprado
        }).ToList();

        var idGenerado = await repository.CreateAsync(nuevaOrden, listaDetalles);

        // Retornamos la respuesta construida
        return new OrdenResponseDto(
            idGenerado,
            nuevaOrden.IdUsuario,
            nuevaOrden.MontoTotal,
            nuevaOrden.Estado,
            nuevaOrden.MetodoPago,
            DateTime.UtcNow,
            listaDetalles.Select(d => new DetalleOrdenResponseDto(0, d.IdJuego, d.PrecioComprado)).ToList()
        );
    }

    public async Task<bool> UpdateEstadoAsync(int idOrden, string nuevoEstado)
    {
        return await repository.UpdateEstadoAsync(idOrden, nuevoEstado);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }

    private static OrdenResponseDto MapToDto(Orden o) => new(
        o.IdOrden,
        o.IdUsuario,
        o.MontoTotal,
        o.Estado,
        o.MetodoPago,
        o.FechaCreacion,
        o.Detalles.Select(d => new DetalleOrdenResponseDto(d.IdDetalle, d.IdJuego, d.PrecioComprado)).ToList()
    );
}