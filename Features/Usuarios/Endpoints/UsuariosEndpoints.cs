using APIVideoJuegos.Features.Usuarios.DTOS;
using APIVideoJuegos.Features.Usuarios.service;
using MySql.Data.MySqlClient;

namespace APIVideoJuegos.Features.Usuarios.Endpoints;

public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/usuarios")
                       .WithTags("Usuarios");

        // METHOD: GET
        // ROUTE: /api/usuarios
        group.MapGet("/", async (IUsuarioService service) =>
        {
            var usuarios = await service.GetAllAsync();
            return Results.Ok(usuarios);
        });

        // METHOD: GET
        // ROUTE: /api/usuarios/{id}
        group.MapGet("/{id:int}", async (int id, IUsuarioService service) =>
        {
            var usuario = await service.GetByIdAsync(id);
            return usuario is not null 
                ? Results.Ok(usuario) 
                : Results.NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });
        });

        // METHOD: POST
        // ROUTE: /api/usuarios
        group.MapPost("/", async (CreateUsuarioDto dto, IUsuarioService service) =>
        {
            try
            {
                var creado = await service.CreateAsync(dto);
                return Results.Created($"/api/usuarios/{creado.IdUsuario}", new 
                { 
                    mensaje = "Usuario creado exitosamente.", 
                    datos = creado 
                });
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062) // Duplicate entry
                {
                    return Results.Conflict(new { error = "El nombre de usuario o email ya se encuentra registrado.", detalle = ex.Message });
                }
                if (ex.Number == 1452) // Foreign key fails (id_empresa)
                {
                    return Results.BadRequest(new { error = $"La empresa con ID {dto.IdEmpresa} no existe.", detalle = ex.Message });
                }
                return Results.BadRequest(new { error = "Error en la base de datos al registrar el usuario.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: PUT
        // ROUTE: /api/usuarios/{id}
        group.MapPut("/{id:int}", async (int id, UpdateUsuarioDto dto, IUsuarioService service) =>
        {
            try
            {
                var actualizado = await service.UpdateAsync(id, dto);
                return actualizado 
                    ? Results.Ok(new { mensaje = $"Usuario con ID {id} actualizado exitosamente." }) 
                    : Results.NotFound(new { mensaje = $"Usuario con ID {id} no encontrado para actualizar." });
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    return Results.Conflict(new { error = "El nombre de usuario o email ya está en uso por otro usuario.", detalle = ex.Message });
                }
                if (ex.Number == 1452)
                {
                    return Results.BadRequest(new { error = $"La empresa con ID {dto.IdEmpresa} no existe.", detalle = ex.Message });
                }
                return Results.BadRequest(new { error = "Error en la base de datos al actualizar el usuario.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: DELETE
        // ROUTE: /api/usuarios/{id}
        group.MapDelete("/{id:int}", async (int id, IUsuarioService service) =>
        {
            try
            {
                var eliminado = await service.DeleteAsync(id);
                return eliminado 
                    ? Results.Ok(new { mensaje = $"Usuario con ID {id} eliminado exitosamente." }) 
                    : Results.NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "No se puede eliminar el usuario debido a registros asociados (órdenes o biblioteca).", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
    }
}