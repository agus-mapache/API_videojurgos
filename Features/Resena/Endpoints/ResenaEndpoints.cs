using APIVideoJuegos.Features.Resena.DTOs;
using APIVideoJuegos.Features.Resena.Service;

namespace APIVideoJuegos.Features.Resena.Endpoints;

public static class ResenaEndpoints
{
    public static void MapResenaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/resenas")
                       .WithTags("Resenas");

        // Obtener una reseña por su ID
        group.MapGet("/{id:int}", async (int id, IResenaService service) =>
        {
            var resena = await service.GetByIdAsync(id);
            return resena is not null ? Results.Ok(resena) : Results.NotFound();
        });

        // Obtener todas las reseñas de un juego en específico (ideal para la ficha del juego)
        group.MapGet("/juego/{idJuego:int}", async (int idJuego, IResenaService service) =>
        {
            var resenas = await service.GetByJuegoAsync(idJuego);
            return Results.Ok(resenas);
        });

        // Obtener todas las reseñas hechas por un usuario
        group.MapGet("/usuario/{idUsuario:int}", async (int idUsuario, IResenaService service) =>
        {
            var resenas = await service.GetByUsuarioAsync(idUsuario);
            return Results.Ok(resenas);
        });

        // Crear una nueva reseña
        group.MapPost("/", async (CreateResenaDto dto, IResenaService service) =>
        {
            var creada = await service.CreateAsync(dto);
            return Results.Created($"/api/resenas/{creada.IdResena}", creada);
        });

        // Actualizar una reseña existente
        group.MapPut("/{id:int}", async (int id, UpdateResenaDto dto, IResenaService service) =>
        {
            var actualizado = await service.UpdateAsync(id, dto);
            return actualizado ? Results.NoContent() : Results.NotFound();
        });

        // Eliminar una reseña
        group.MapDelete("/{id:int}", async (int id, IResenaService service) =>
        {
            var eliminado = await service.DeleteAsync(id);
            return eliminado ? Results.NoContent() : Results.NotFound();
        });
    }
}