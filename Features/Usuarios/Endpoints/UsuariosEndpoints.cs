using APIVideoJuegos.Features.Usuarios.DTOS;
using APIVideoJuegos.Features.Usuarios.service;

namespace APIVideoJuegos.Features.Usuarios.Endpoints;

public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/usuarios")
                       .WithTags("Usuarios");

        group.MapGet("/", async (IUsuarioService service) =>
        {
            var usuarios = await service.GetAllAsync();
            return Results.Ok(usuarios);
        });

        group.MapGet("/{id:int}", async (int id, IUsuarioService service) =>
        {
            var usuario = await service.GetByIdAsync(id);
            return usuario is not null ? Results.Ok(usuario) : Results.NotFound();
        });

        group.MapPost("/", async (CreateUsuarioDto dto, IUsuarioService service) =>
        {
            var creado = await service.CreateAsync(dto);
            return Results.Created($"/api/usuarios/{creado.IdUsuario}", creado);
        });

        group.MapPut("/{id:int}", async (int id, UpdateUsuarioDto dto, IUsuarioService service) =>
        {
            var actualizado = await service.UpdateAsync(id, dto);
            return actualizado ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, IUsuarioService service) =>
        {
            var eliminado = await service.DeleteAsync(id);
            return eliminado ? Results.NoContent() : Results.NotFound();
        });
    }
}