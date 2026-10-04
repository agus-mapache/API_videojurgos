using APIVideoJuegos.Features.Bibloteca.DTOs;
using APIVideoJuegos.Features.Bibloteca.Service;

namespace APIVideoJuegos.Features.Bibloteca.Endpoints;

public static class BibliotecaEndpoints
{
    public static void MapBibliotecaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/biblioteca")
                       .WithTags("Biblioteca");

        // Obtener toda la biblioteca de un usuario específico
        group.MapGet("/usuario/{idUsuario:int}", async (int idUsuario, IBibliotecaService service) =>
        {
            var biblioteca = await service.GetByUsuarioAsync(idUsuario);
            return Results.Ok(biblioteca);
        });

        // Verificar si un usuario tiene un juego en específico
        group.MapGet("/usuario/{idUsuario:int}/juego/{idJuego:int}", async (int idUsuario, int idJuego, IBibliotecaService service) =>
        {
            var item = await service.GetJuegoEnBibliotecaAsync(idUsuario, idJuego);
            return item is not null ? Results.Ok(item) : Results.NotFound();
        });

        // Agregar un juego a la biblioteca (ej. cuando se completa una orden de compra)
        group.MapPost("/", async (AddJuegoBibliotecaDto dto, IBibliotecaService service) =>
        {
            var agregado = await service.AddJuegoAsync(dto);
            return agregado ? Results.Created($"/api/biblioteca/usuario/{dto.IdUsuario}/juego/{dto.IdJuego}", new { mensaje = "Juego añadido a la biblioteca" })
                            : Results.BadRequest(new { mensaje = "No se pudo añadir (el usuario ya posee el juego o los IDs son inválidos)" });
        });

        // Actualizar horas jugadas (ej. métricas de juego)
        group.MapPatch("/usuario/{idUsuario:int}/juego/{idJuego:int}/horas", async (int idUsuario, int idJuego, UpdateHorasDto dto, IBibliotecaService service) =>
        {
            var actualizado = await service.UpdateHorasAsync(idUsuario, idJuego, dto);
            return actualizado ? Results.NoContent() : Results.NotFound();
        });

        // Quitar un juego de la biblioteca
        group.MapDelete("/usuario/{idUsuario:int}/juego/{idJuego:int}", async (int idUsuario, int idJuego, IBibliotecaService service) =>
        {
            var eliminado = await service.RemoveJuegoAsync(idUsuario, idJuego);
            return eliminado ? Results.NoContent() : Results.NotFound();
        });
    }
}