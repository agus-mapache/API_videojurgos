using APIVideoJuegos.Features.Generos.DTOS;
using APIVideoJuegos.Features.Generos.Service;

namespace APIVideoJuegos.Features.Generos.Endpoints;

public static class GeneroEndpoints
{
    public static void MapGeneroEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/generos")
                       .WithTags("Generos");

        // --- CRUD Estándar ---
        group.MapGet("/", async (IGeneroService service) => 
            Results.Ok(await service.GetAllAsync()));

        group.MapGet("/{id:int}", async (int id, IGeneroService service) =>
        {
            var res = await service.GetByIdAsync(id);
            return res is not null ? Results.Ok(res) : Results.NotFound();
        });

        group.MapPost("/", async (CreateGeneroDto dto, IGeneroService service) =>
{
            try
            {
                var creado = await service.CreateAsync(dto);
                return Results.Created($"/api/generos/{creado.IdGenero}", creado);
            }
            catch (InvalidOperationException ex)
            {
                // Devuelve un 400 Bad Request amigable en lugar de un error 500
                return Results.BadRequest(new { mensaje = ex.Message });
            }
        });

        group.MapPut("/{id:int}", async (int id, UpdateGeneroDto dto, IGeneroService service) =>
        {
            var actualizado = await service.UpdateAsync(id, dto);
            return actualizado ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, IGeneroService service) =>
        {
            var eliminado = await service.DeleteAsync(id);
            return eliminado ? Results.NoContent() : Results.NotFound();
        });

        // --- Endpoints de la Relación con Juegos ---
        
        // Obtener los géneros que pertenecen a un juego específico
        group.MapGet("/juego/{idJuego:int}", async (int idJuego, IGeneroService service) =>
        {
            var generos = await service.GetGenerosByJuegoAsync(idJuego);
            return Results.Ok(generos);
        });

        // Asignar un género a un juego
        group.MapPost("/juego/asociar", async (AsignarGeneroJuegoDto dto, IGeneroService service) =>
        {
            var exito = await service.AsignarGeneroAJuegoAsync(dto);
            return exito ? Results.Ok(new { mensaje = "Género asociado al juego exitosamente" }) 
                         : Results.BadRequest(new { mensaje = "No se pudo asociar (quizás ya exista o el ID no es válido)" });
        });

        // Desasociar un género de un juego
        group.MapDelete("/juego/{idJuego:int}/genero/{idGenero:int}", async (int idJuego, int idGenero, IGeneroService service) =>
        {
            var exito = await service.RemoverGeneroDeJuegoAsync(idJuego, idGenero);
            return exito ? Results.NoContent() : Results.NotFound();
        });
    }
}