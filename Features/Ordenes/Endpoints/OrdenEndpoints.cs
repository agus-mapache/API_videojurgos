using APIVideoJuegos.Features.Ordenes.DTOs;
using APIVideoJuegos.Features.Ordenes.Service;
using MySql.Data.MySqlClient;

namespace APIVideoJuegos.Features.Ordenes.Endpoints;

public static class OrdenEndpoints
{
    public static void MapOrdenEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ordenes")
                       .WithTags("Ordenes");

        // METHOD: GET
        // ROUTE: /api/ordenes
        group.MapGet("/", async (IOrdenService service) =>
        {
            var ordenes = await service.GetAllAsync();
            return Results.Ok(ordenes);
        });

        // METHOD: GET
        // ROUTE: /api/ordenes/{id}
        group.MapGet("/{id:int}", async (int id, IOrdenService service) =>
        {
            var orden = await service.GetByIdAsync(id);
            return orden is not null 
                ? Results.Ok(orden) 
                : Results.NotFound(new { mensaje = $"Orden con ID {id} no encontrada." });
        });

        // METHOD: GET
        // ROUTE: /api/ordenes/usuario/{idUsuario}
        group.MapGet("/usuario/{idUsuario:int}", async (int idUsuario, IOrdenService service) =>
        {
            var ordenes = await service.GetByUsuarioAsync(idUsuario);
            return Results.Ok(ordenes);
        });

        // METHOD: POST
        // ROUTE: /api/ordenes
        group.MapPost("/", async (CreateOrdenDto dto, IOrdenService service) =>
        {
            try
            {
                var creada = await service.CreateAsync(dto);
                return Results.Created($"/api/ordenes/{creada.IdOrden}", new 
                { 
                    mensaje = "Orden creada exitosamente.", 
                    datos = creada 
                });
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1452)
                {
                    return Results.BadRequest(new { error = "El usuario o uno de los juegos especificados en la orden no existe.", detalle = ex.Message });
                }
                return Results.BadRequest(new { error = "Error en la base de datos al crear la orden.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: PATCH
        // ROUTE: /api/ordenes/{id}/estado
        group.MapPatch("/{id:int}/estado", async (int id, UpdateEstadoOrdenDto dto, IOrdenService service) =>
        {
            try
            {
                var actualizado = await service.UpdateEstadoAsync(id, dto.Estado);
                return actualizado 
                    ? Results.Ok(new { mensaje = $"Estado de la orden {id} actualizado a '{dto.Estado}' exitosamente." }) 
                    : Results.NotFound(new { mensaje = $"Orden con ID {id} no encontrada para actualizar." });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "Error en la base de datos al actualizar el estado de la orden.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: DELETE
        // ROUTE: /api/ordenes/{id}
        group.MapDelete("/{id:int}", async (int id, IOrdenService service) =>
        {
            try
            {
                var eliminado = await service.DeleteAsync(id);
                return eliminado 
                    ? Results.Ok(new { mensaje = $"Orden con ID {id} eliminada exitosamente." }) 
                    : Results.NotFound(new { mensaje = $"Orden con ID {id} no encontrada." });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "Error en la base de datos al eliminar la orden.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
    }
}