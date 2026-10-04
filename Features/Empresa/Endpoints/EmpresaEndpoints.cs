using APIVideoJuegos.Features.Empresa.DTOS;
using APIVideoJuegos.Features.Empresa.Service;

namespace APIVideoJuegos.Features.Empresa.Endpoints;

public static class EmpresaEndpoints
{
    public static void MapEmpresaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/empresas")
                       .WithTags("Empresas");

        group.MapGet("/", async (IEmpresaService service) =>
        {
            var empresas = await service.GetAllAsync();
            return Results.Ok(empresas);
        });

        group.MapGet("/{id:int}", async (int id, IEmpresaService service) =>
        {
            var empresa = await service.GetByIdAsync(id);
            return empresa is not null ? Results.Ok(empresa) : Results.NotFound();
        });

        group.MapPost("/", async (CreateEmpresaDto dto, IEmpresaService service) =>
        {
            var creado = await service.CreateAsync(dto);
            return Results.Created($"/api/empresas/{creado.IdEmpresa}", creado);
        });

        group.MapPut("/{id:int}", async (int id, UpdateEmpresaDto dto, IEmpresaService service) =>
        {
            var actualizado = await service.UpdateAsync(id, dto);
            return actualizado ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, IEmpresaService service) =>
        {
            var eliminado = await service.DeleteAsync(id);
            return eliminado ? Results.NoContent() : Results.NotFound();
        });
    }
}