using APIVideoJuegos.Features.Resena.DTOs;
using APIVideoJuegos.Features.Resena.Repository;

namespace APIVideoJuegos.Features.Resena.Service;
public class ResenaService(IResenaRepository repository) : IResenaService
{
    public async Task<IEnumerable<ResenaResponseDto>> GetByJuegoAsync(int idJuego)
    {
        var resenas = await repository.GetByJuegoIdAsync(idJuego);
        return resenas.Select(MapToDto);
    }

    public async Task<IEnumerable<ResenaResponseDto>> GetByUsuarioAsync(int idUsuario)
    {
        var resenas = await repository.GetByUsuarioIdAsync(idUsuario);
        return resenas.Select(MapToDto);
    }

    public async Task<ResenaResponseDto?> GetByIdAsync(int id)
    {
        var resena = await repository.GetByIdAsync(id);
        return resena is null ? null : MapToDto(resena);
    }

    public async Task<ResenaResponseDto> CreateAsync(CreateResenaDto dto)
    {
        var nuevaResena = new Resena
        {
            IdUsuario = dto.IdUsuario,
            IdJuego = dto.IdJuego,
            Recomendado = dto.Recomendado,
            Comentario = dto.Comentario
        };

        var idGenerado = await repository.CreateAsync(nuevaResena);

        return new ResenaResponseDto(
            idGenerado,
            nuevaResena.IdUsuario,
            nuevaResena.IdJuego,
            nuevaResena.Recomendado,
            nuevaResena.Comentario,
            DateTime.UtcNow
        );
    }

    public async Task<bool> UpdateAsync(int id, UpdateResenaDto dto)
    {
        var existente = await repository.GetByIdAsync(id);
        if (existente is null) return false;

        existente.Recomendado = dto.Recomendado;
        existente.Comentario = dto.Comentario;

        return await repository.UpdateAsync(existente);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }

    private static ResenaResponseDto MapToDto(Resena r) => new(
        r.IdResena,
        r.IdUsuario,
        r.IdJuego,
        r.Recomendado,
        r.Comentario,
        r.FechaPublicacion
    );
}