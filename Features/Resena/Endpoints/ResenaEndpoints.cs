using APIVideoJuegos.Features.Resena.DTOs;
using APIVideoJuegos.Features.Resena.Service;
using MySql.Data.MySqlClient;

namespace APIVideoJuegos.Features.Resena.Endpoints;

public static class ResenaEndpoints
{
    public static void MapResenaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/resenas")
                       .WithTags("Resenas");

        // METHOD: GET
        // ROUTE: /api/resenas/{id}
        group.MapGet("/{id:int}", async (int id, IResenaService service) =>
        {
            var resena = await service.GetByIdAsync(id);
            return resena is not null 
                ? Results.Ok(resena) 
                : Results.NotFound(new { mensaje = $"Reseña con ID {id} no encontrada." });
        });

        // METHOD: GET
        // ROUTE: /api/resenas/juego/{idJuego}
        group.MapGet("/juego/{idJuego:int}", async (int idJuego, IResenaService service) =>
        {
            var resenas = await service.GetByJuegoAsync(idJuego);
            return Results.Ok(resenas);
        });

        // METHOD: GET
        // ROUTE: /api/resenas/usuario/{idUsuario}
        group.MapGet("/usuario/{idUsuario:int}", async (int idUsuario, IResenaService service) =>
        {
            var resenas = await service.GetByUsuarioAsync(idUsuario);
            return Results.Ok(resenas);
        });

        // METHOD: POST
        // ROUTE: /api/resenas
        group.MapPost("/", async (CreateResenaDto dto, IResenaService service) =>
        {
            try
            {
                var creada = await service.CreateAsync(dto);
                return Results.Created($"/api/resenas/{creada.IdResena}", new 
                { 
                    mensaje = "Reseña creada exitosamente.", 
                    datos = creada 
                });
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1452)
                {
                    return Results.BadRequest(new { error = "El usuario o el juego especificado no existe.", detalle = ex.Message });
                }
                return Results.BadRequest(new { error = "Error en la base de datos al crear la reseña.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: PUT
        // ROUTE: /api/resenas/{id}
        group.MapPut("/{id:int}", async (int id, UpdateResenaDto dto, IResenaService service) =>
        {
            try
            {
                var actualizado = await service.UpdateAsync(id, dto);
                return actualizado 
                    ? Results.Ok(new { mensaje = $"Reseña con ID {id} actualizada exitosamente." }) 
                    : Results.NotFound(new { mensaje = $"Reseña con ID {id} no encontrada para actualizar." });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "Error en la base de datos al actualizar la reseña.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: DELETE
        // ROUTE: /api/resenas/{id}
        group.MapDelete("/{id:int}", async (int id, IResenaService service) =>
        {
            try
            {
                var eliminado = await service.DeleteAsync(id);
                return eliminado 
                    ? Results.Ok(new { mensaje = $"Reseña con ID {id} eliminada exitosamente." }) 
                    : Results.NotFound(new { mensaje = $"Reseña con ID {id} no encontrada." });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "Error en la base de datos al eliminar la reseña.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
    }
}
