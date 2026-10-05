using APIVideoJuegos.Features.Generos.DTOS;
using APIVideoJuegos.Features.Generos.Service;
using MySql.Data.MySqlClient;

namespace APIVideoJuegos.Features.Generos.Endpoints;

public static class GeneroEndpoints
{
    public static void MapGeneroEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/generos")
                       .WithTags("Generos");

        // METHOD: GET
        // ROUTE: /api/generos
        group.MapGet("/", async (IGeneroService service) =>
        {
            var generos = await service.GetAllAsync();
            return Results.Ok(generos);
        });

        // METHOD: GET
        // ROUTE: /api/generos/{id}
        group.MapGet("/{id:int}", async (int id, IGeneroService service) =>
        {
            var res = await service.GetByIdAsync(id);
            return res is not null 
                ? Results.Ok(res) 
                : Results.NotFound(new { mensaje = $"Género con ID {id} no encontrado." });
        });

        // METHOD: POST
        // ROUTE: /api/generos
        group.MapPost("/", async (CreateGeneroDto dto, IGeneroService service) =>
        {
            try
            {
                var creado = await service.CreateAsync(dto);
                return Results.Created($"/api/generos/{creado.IdGenero}", new 
                { 
                    mensaje = "Género creado exitosamente.", 
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
                    return Results.Conflict(new { error = $"El género '{dto.Nombre}' ya existe.", detalle = ex.Message });
                }
                return Results.BadRequest(new { error = "Error en la base de datos al crear el género.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: PUT
        // ROUTE: /api/generos/{id}
        group.MapPut("/{id:int}", async (int id, UpdateGeneroDto dto, IGeneroService service) =>
        {
            try
            {
                var actualizado = await service.UpdateAsync(id, dto);
                return actualizado 
                    ? Results.Ok(new { mensaje = $"Género con ID {id} actualizado exitosamente." }) 
                    : Results.NotFound(new { mensaje = $"Género con ID {id} no encontrado para actualizar." });
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    return Results.Conflict(new { error = $"El nombre de género '{dto.Nombre}' ya está en uso.", detalle = ex.Message });
                }
                return Results.BadRequest(new { error = "Error en la base de datos al actualizar el género.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: DELETE
        // ROUTE: /api/generos/{id}
        group.MapDelete("/{id:int}", async (int id, IGeneroService service) =>
        {
            try
            {
                var eliminado = await service.DeleteAsync(id);
                return eliminado 
                    ? Results.Ok(new { mensaje = $"Género con ID {id} eliminado exitosamente." }) 
                    : Results.NotFound(new { mensaje = $"Género con ID {id} no encontrado." });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "No se puede eliminar el género debido a asociaciones existentes.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: GET
        // ROUTE: /api/generos/juego/{idJuego}
        group.MapGet("/juego/{idJuego:int}", async (int idJuego, IGeneroService service) =>
        {
            var generos = await service.GetGenerosByJuegoAsync(idJuego);
            return Results.Ok(generos);
        });

        // METHOD: POST
        // ROUTE: /api/generos/juego/asociar
        group.MapPost("/juego/asociar", async (AsignarGeneroJuegoDto dto, IGeneroService service) =>
        {
            try
            {
                var exito = await service.AsignarGeneroAJuegoAsync(dto);
                return exito 
                    ? Results.Ok(new { mensaje = $"Género {dto.IdGenero} asociado exitosamente al juego {dto.IdJuego}." }) 
                    : Results.BadRequest(new { error = "No se pudo asociar (verifique si los IDs son válidos o si ya se encuentra asociado)." });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "Error en la base de datos al asociar el género con el juego.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: DELETE
        // ROUTE: /api/generos/juego/{idJuego}/genero/{idGenero}
        group.MapDelete("/juego/{idJuego:int}/genero/{idGenero:int}", async (int idJuego, int idGenero, IGeneroService service) =>
        {
            try
            {
                var exito = await service.RemoverGeneroDeJuegoAsync(idJuego, idGenero);
                return exito 
                    ? Results.Ok(new { mensaje = $"Género {idGenero} desasociado exitosamente del juego {idJuego}." }) 
                    : Results.NotFound(new { mensaje = "No se encontró la asociación entre el juego y el género para eliminar." });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "Error en la base de datos al desasociar el género.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
    }
}