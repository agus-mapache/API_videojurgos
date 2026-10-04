using APIVideoJuegos.Features.Juegos.DTOS;
using APIVideoJuegos.Features.Juegos.Service;

namespace APIVideoJuegos.Features.Juegos.Endpoints;

public static class JuegoEndpoints
{
    public static void MapJuegoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/juegos")
                       .WithTags("Juegos");

        group.MapGet("/", async (IJuegoService service) =>
        {
            var juegos = await service.GetAllAsync();
            return Results.Ok(juegos);
        });

        group.MapGet("/{id:int}", async (int id, IJuegoService service) =>
        {
            var juego = await service.GetByIdAsync(id);
            return juego is not null ? Results.Ok(juego) : Results.NotFound();
        });

        // Nuevo endpoint para buscar por slug
        group.MapGet("/slug/{slug}", async (string slug, IJuegoService service) =>
        {
            var juego = await service.GetBySlugAsync(slug);
            return juego is not null ? Results.Ok(juego) : Results.NotFound();
        });

       group.MapPost("/", async (CreateJuegoDto dto, IJuegoService service) =>
        {
            try
            {
                var creado = await service.CreateAsync(dto);
                return Results.Created($"/api/juegos/{creado.IdJuego}", creado);
            }
            catch (InvalidOperationException ex)
            {
                // Devuelve un error 400 Bad Request con el mensaje de validación
                return Results.BadRequest(new { Error = ex.Message });
            }
        });

        group.MapPut("/{id:int}", async (int id, UpdateJuegoDto dto, IJuegoService service) =>
        {
            var actualizado = await service.UpdateAsync(id, dto);
            return actualizado ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, IJuegoService service) =>
        {
            var eliminado = await service.DeleteAsync(id);
            return eliminado ? Results.NoContent() : Results.NotFound();
        });
    }
}