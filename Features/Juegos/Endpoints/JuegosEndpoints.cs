using APIVideoJuegos.Features.Juegos.DTOS;
using APIVideoJuegos.Features.Juegos.Service;
using MySql.Data.MySqlClient;

namespace APIVideoJuegos.Features.Juegos.Endpoints;

public static class JuegoEndpoints
{
    public static void MapJuegoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/juegos")
                       .WithTags("Juegos");

        // METHOD: GET
        // ROUTE: /api/juegos
        group.MapGet("/", async (IJuegoService service) =>
        {
            var juegos = await service.GetAllAsync();
            return Results.Ok(juegos);
        });

        // METHOD: GET
        // ROUTE: /api/juegos/{id}
        group.MapGet("/{id:int}", async (int id, IJuegoService service) =>
        {
            var juego = await service.GetByIdAsync(id);
            return juego is not null 
                ? Results.Ok(juego) 
                : Results.NotFound(new { mensaje = $"Juego con ID {id} no encontrado." });
        });

        // METHOD: GET
        // ROUTE: /api/juegos/slug/{slug}
        group.MapGet("/slug/{slug}", async (string slug, IJuegoService service) =>
        {
            var juego = await service.GetBySlugAsync(slug);
            return juego is not null 
                ? Results.Ok(juego) 
                : Results.NotFound(new { mensaje = $"Juego con slug '{slug}' no encontrado." });
        });

        // METHOD: POST
        // ROUTE: /api/juegos
        group.MapPost("/", async (CreateJuegoDto dto, IJuegoService service) =>
        {
            try
            {
                var creado = await service.CreateAsync(dto);
                return Results.Created($"/api/juegos/{creado.IdJuego}", new 
                { 
                    mensaje = "Juego creado exitosamente.", 
                    datos = creado 
                });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    return Results.Conflict(new { error = $"Ya existe un juego con el slug '{dto.Slug}'.", detalle = ex.Message });
                }
                if (ex.Number == 1452)
                {
                    return Results.BadRequest(new { error = "El desarrollador o editor especificado no existe.", detalle = ex.Message });
                }
                return Results.BadRequest(new { error = "Error en la base de datos al registrar el juego.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: PUT
        // ROUTE: /api/juegos/{id}
        group.MapPut("/{id:int}", async (int id, UpdateJuegoDto dto, IJuegoService service) =>
        {
            try
            {
                var actualizado = await service.UpdateAsync(id, dto);
                return actualizado 
                    ? Results.Ok(new { mensaje = $"Juego con ID {id} actualizado exitosamente." }) 
                    : Results.NotFound(new { mensaje = $"Juego con ID {id} no encontrado para actualizar." });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    return Results.Conflict(new { error = $"El slug '{dto.Slug}' ya está en uso.", detalle = ex.Message });
                }
                if (ex.Number == 1452)
                {
                    return Results.BadRequest(new { error = "El desarrollador o editor especificado no existe.", detalle = ex.Message });
                }
                return Results.BadRequest(new { error = "Error en la base de datos al actualizar el juego.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: DELETE
        // ROUTE: /api/juegos/{id}
        group.MapDelete("/{id:int}", async (int id, IJuegoService service) =>
        {
            try
            {
                var eliminado = await service.DeleteAsync(id);
                return eliminado 
                    ? Results.Ok(new { mensaje = $"Juego con ID {id} eliminado exitosamente." }) 
                    : Results.NotFound(new { mensaje = $"Juego con ID {id} no encontrado." });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "No se puede eliminar el juego debido a dependencias activas.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
    }
}