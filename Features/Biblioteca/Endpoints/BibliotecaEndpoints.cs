using APIVideoJuegos.Features.Biblioteca.DTOs;
using APIVideoJuegos.Features.Biblioteca.Service;
using MySql.Data.MySqlClient;

namespace APIVideoJuegos.Features.Biblioteca.Endpoints;

public static class BibliotecaEndpoints
{
    public static void MapBibliotecaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/biblioteca")
                       .WithTags("Biblioteca");

        // METHOD: GET
        // ROUTE: /api/biblioteca/usuario/{idUsuario}
        group.MapGet("/usuario/{idUsuario:int}", async (int idUsuario, IBibliotecaService service) =>
        {
            var biblioteca = await service.GetByUsuarioAsync(idUsuario);
            return Results.Ok(biblioteca);
        });

        // METHOD: GET
        // ROUTE: /api/biblioteca/usuario/{idUsuario}/juego/{idJuego}
        group.MapGet("/usuario/{idUsuario:int}/juego/{idJuego:int}", async (int idUsuario, int idJuego, IBibliotecaService service) =>
        {
            var item = await service.GetJuegoEnBibliotecaAsync(idUsuario, idJuego);
            return item is not null 
                ? Results.Ok(item) 
                : Results.NotFound(new { mensaje = $"El usuario {idUsuario} no posee el juego {idJuego} en su biblioteca." });
        });

        // METHOD: POST
        // ROUTE: /api/biblioteca
        group.MapPost("/", async (AddJuegoBibliotecaDto dto, IBibliotecaService service) =>
        {
            try
            {
                var agregado = await service.AddJuegoAsync(dto);
                return agregado 
                    ? Results.Created($"/api/biblioteca/usuario/{dto.IdUsuario}/juego/{dto.IdJuego}", new { mensaje = "Juego añadido a la biblioteca exitosamente.", datos = dto })
                    : Results.BadRequest(new { error = "No se pudo añadir el juego (verifique si ya lo posee o si los IDs son inválidos)." });
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1452)
                {
                    return Results.BadRequest(new { error = "El usuario o el juego especificado no existe.", detalle = ex.Message });
                }
                return Results.BadRequest(new { error = "Error en la base de datos al agregar el juego a la biblioteca.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: PATCH
        // ROUTE: /api/biblioteca/usuario/{idUsuario}/juego/{idJuego}/horas
        group.MapPatch("/usuario/{idUsuario:int}/juego/{idJuego:int}/horas", async (int idUsuario, int idJuego, UpdateHorasDto dto, IBibliotecaService service) =>
        {
            try
            {
                var actualizado = await service.UpdateHorasAsync(idUsuario, idJuego, dto);
                return actualizado 
                    ? Results.Ok(new { mensaje = $"Horas jugadas actualizadas a {dto.HorasJugadas} hrs para el usuario {idUsuario} en el juego {idJuego}." }) 
                    : Results.NotFound(new { mensaje = "No se encontró el juego en la biblioteca del usuario." });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "Error en la base de datos al actualizar las horas jugadas.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: DELETE
        // ROUTE: /api/biblioteca/usuario/{idUsuario}/juego/{idJuego}
        group.MapDelete("/usuario/{idUsuario:int}/juego/{idJuego:int}", async (int idUsuario, int idJuego, IBibliotecaService service) =>
        {
            try
            {
                var eliminado = await service.RemoveJuegoAsync(idUsuario, idJuego);
                return eliminado 
                    ? Results.Ok(new { mensaje = $"Juego {idJuego} removido de la biblioteca del usuario {idUsuario} exitosamente." }) 
                    : Results.NotFound(new { mensaje = "No se encontró el juego en la biblioteca del usuario para remover." });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "Error en la base de datos al remover el juego de la biblioteca.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
    }
}
