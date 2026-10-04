using APIVideoJuegos.Features.Juegos.DTOS;
using APIVideoJuegos.Features.Juegos.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVideoJuegos.Features.Juegos.Service;

public class JuegoService(IJuegoRepository repository /*, IEmpresaRepository empresaRepo */) : IJuegoService
{
    public async Task<IEnumerable<JuegoResponseDto>> GetAllAsync()
    {
        var juegos = await repository.GetAllAsync();
        return juegos.Select(MapToResponseDto);
    }

    public async Task<JuegoResponseDto?> GetByIdAsync(int id)
    {
        var juego = await repository.GetByIdAsync(id);
        return juego is null ? null : MapToResponseDto(juego);
    }

    public async Task<JuegoResponseDto?> GetBySlugAsync(string slug)
    {
        var juego = await repository.GetBySlugAsync(slug);
        return juego is null ? null : MapToResponseDto(juego);
    }

    public async Task<JuegoResponseDto> CreateAsync(CreateJuegoDto dto)
    {
        // 1. Validar que el Slug sea único
        var juegoConMismoSlug = await repository.GetBySlugAsync(dto.Slug);
        if (juegoConMismoSlug != null)
        {
            throw new InvalidOperationException($"Ya existe un juego registrado con el slug '{dto.Slug}'.");
        }

        // 2. Validar Foreign Keys (Descomentar cuando tengas el repositorio de Empresas)
        // var desarrolladorExiste = await empresaRepo.GetByIdAsync(dto.IdDesarrollador);
        // if (desarrolladorExiste == null) 
        //     throw new InvalidOperationException($"El desarrollador con ID {dto.IdDesarrollador} no existe.");

        var nuevoJuego = new Juego
        {
            Titulo = dto.Titulo,
            Slug = dto.Slug,
            Descripcion = dto.Descripcion,
            PrecioBase = dto.PrecioBase,
            DescuentoPorcentaje = dto.DescuentoPorcentaje,
            FechaLanzamiento = dto.FechaLanzamiento,
            IdDesarrollador = dto.IdDesarrollador,
            IdEditor = dto.IdEditor
        };

        var idGenerado = await repository.CreateAsync(nuevoJuego);
        
        return new JuegoResponseDto(
            idGenerado, nuevoJuego.Titulo, nuevoJuego.Slug, 
            nuevoJuego.Descripcion, nuevoJuego.PrecioBase, 
            nuevoJuego.DescuentoPorcentaje, nuevoJuego.FechaLanzamiento, 
            nuevoJuego.IdDesarrollador, nuevoJuego.IdEditor
        );
    }

    public async Task<bool> UpdateAsync(int id, UpdateJuegoDto dto)
    {
        var juegoExistente = await repository.GetByIdAsync(id);
        if (juegoExistente is null) return false;

        // 1. Validar el Slug (Solo si el usuario intenta cambiarlo a uno que ya existe)
        if (juegoExistente.Slug != dto.Slug)
        {
            var juegoConMismoSlug = await repository.GetBySlugAsync(dto.Slug);
            if (juegoConMismoSlug != null)
            {
                throw new InvalidOperationException($"El slug '{dto.Slug}' ya está siendo utilizado por otro juego.");
            }
        }

        juegoExistente.Titulo = dto.Titulo;
        juegoExistente.Slug = dto.Slug;
        juegoExistente.Descripcion = dto.Descripcion;
        juegoExistente.PrecioBase = dto.PrecioBase;
        juegoExistente.DescuentoPorcentaje = dto.DescuentoPorcentaje;
        juegoExistente.FechaLanzamiento = dto.FechaLanzamiento;
        juegoExistente.IdDesarrollador = dto.IdDesarrollador;
        juegoExistente.IdEditor = dto.IdEditor;

        return await repository.UpdateAsync(juegoExistente);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }

    private static JuegoResponseDto MapToResponseDto(Juego j) =>
        new(
            j.IdJuego, j.Titulo, j.Slug, j.Descripcion, 
            j.PrecioBase, j.DescuentoPorcentaje, j.FechaLanzamiento, 
            j.IdDesarrollador, j.IdEditor
        );
}