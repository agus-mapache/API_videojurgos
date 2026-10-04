using APIVideoJuegos.Features.Ordenes.DTOs;
using APIVideoJuegos.Features.Ordenes.Service;

namespace APIVideoJuegos.Features.Ordenes.Endpoints;

public static class OrdenEndpoints
{
    public static void MapOrdenEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ordenes")
                       .WithTags("Ordenes");

        group.MapGet("/", async (IOrdenService service) =>
        {
            var ordenes = await service.GetAllAsync();
            return Results.Ok(ordenes);
        });

        group.MapGet("/{id:int}", async (int id, IOrdenService service) =>
        {
            var orden = await service.GetByIdAsync(id);
            return orden is not null ? Results.Ok(orden) : Results.NotFound();
        });

        // Buscar órdenes por usuario (muy útil para el perfil de la tienda)
        group.MapGet("/usuario/{idUsuario:int}", async (int idUsuario, IOrdenService service) =>
        {
            var ordenes = await service.GetByUsuarioAsync(idUsuario);
            return Results.Ok(ordenes);
        });

        group.MapPost("/", async (CreateOrdenDto dto, IOrdenService service) =>
        {
            var creada = await service.CreateAsync(dto);
            return Results.Created($"/api/ordenes/{creada.IdOrden}", creada);
        });

        // Endpoint específico para actualizar solo el estado de la orden (ej: 'Completado')
        group.MapPatch("/{id:int}/estado", async (int id, string nuevoEstado, IOrdenService service) =>
        {
            var actualizado = await service.UpdateEstadoAsync(id, nuevoEstado);
            return actualizado ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, IOrdenService service) =>
        {
            var eliminado = await service.DeleteAsync(id);
            return eliminado ? Results.NoContent() : Results.NotFound();
        });
    }
}