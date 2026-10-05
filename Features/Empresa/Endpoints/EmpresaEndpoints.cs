using APIVideoJuegos.Features.Empresa.DTOS;
using APIVideoJuegos.Features.Empresa.Service;
using MySql.Data.MySqlClient;

namespace APIVideoJuegos.Features.Empresa.Endpoints;

public static class EmpresaEndpoints
{
    public static void MapEmpresaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/empresas")
                       .WithTags("Empresas");

        // METHOD: GET
        // ROUTE: /api/empresas
        group.MapGet("/", async (IEmpresaService service) =>
        {
            var empresas = await service.GetAllAsync();
            return Results.Ok(empresas);
        });

        // METHOD: GET
        // ROUTE: /api/empresas/{id}
        group.MapGet("/{id:int}", async (int id, IEmpresaService service) =>
        {
            var empresa = await service.GetByIdAsync(id);
            return empresa is not null 
                ? Results.Ok(empresa) 
                : Results.NotFound(new { mensaje = $"Empresa con ID {id} no encontrada." });
        });

        // METHOD: POST
        // ROUTE: /api/empresas
        group.MapPost("/", async (CreateEmpresaDto dto, IEmpresaService service) =>
        {
            try
            {
                var creado = await service.CreateAsync(dto);
                return Results.Created($"/api/empresas/{creado.IdEmpresa}", new 
                { 
                    mensaje = "Empresa creada exitosamente.", 
                    datos = creado 
                });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "Error en la base de datos al crear la empresa.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: PUT
        // ROUTE: /api/empresas/{id}
        group.MapPut("/{id:int}", async (int id, UpdateEmpresaDto dto, IEmpresaService service) =>
        {
            try
            {
                var actualizado = await service.UpdateAsync(id, dto);
                return actualizado 
                    ? Results.Ok(new { mensaje = $"Empresa con ID {id} actualizada exitosamente." }) 
                    : Results.NotFound(new { mensaje = $"Empresa con ID {id} no encontrada para actualizar." });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "Error en la base de datos al actualizar la empresa.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: DELETE
        // ROUTE: /api/empresas/{id}
        group.MapDelete("/{id:int}", async (int id, IEmpresaService service) =>
        {
            try
            {
                var eliminado = await service.DeleteAsync(id);
                return eliminado 
                    ? Results.Ok(new { mensaje = $"Empresa con ID {id} eliminada exitosamente." }) 
                    : Results.NotFound(new { mensaje = $"Empresa con ID {id} no encontrada." });
            }
            catch (MySqlException ex)
            {
                return Results.BadRequest(new { error = "No se puede eliminar la empresa debido a relaciones activas en la base de datos.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
    }
}