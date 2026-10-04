using APIVideoJuegos.Features.Bibloteca.DTOs;
using APIVideoJuegos.Features.Bibloteca.Repository;

namespace APIVideoJuegos.Features.Bibloteca.Service;

public class BibliotecaService(IBibliotecaRepository repository) : IBibliotecaService
{
    public async Task<IEnumerable<BibliotecaResponseDto>> GetByUsuarioAsync(int idUsuario)
    {
        var items = await repository.GetByUsuarioIdAsync(idUsuario);
        return items.Select(MapToDto);
    }

    public async Task<BibliotecaResponseDto?> GetJuegoEnBibliotecaAsync(int idUsuario, int idJuego)
    {
        var item = await repository.GetAsync(idUsuario, idJuego);
        return item is null ? null : MapToDto(item);
    }

    public async Task<bool> AddJuegoAsync(AddJuegoBibliotecaDto dto)
    {
        var nuevoItem = new BibliotecaItem
        {
            IdUsuario = dto.IdUsuario,
            IdJuego = dto.IdJuego,
            HorasJugadas = 0.0m
        };
        return await repository.AddAsync(nuevoItem);
    }

    public async Task<bool> UpdateHorasAsync(int idUsuario, int idJuego, UpdateHorasDto dto)
    {
        return await repository.UpdateHorasAsync(idUsuario, idJuego, dto.HorasJugadas);
    }

    public async Task<bool> RemoveJuegoAsync(int idUsuario, int idJuego)
    {
        return await repository.RemoveAsync(idUsuario, idJuego);
    }

    private static BibliotecaResponseDto MapToDto(BibliotecaItem b) => new(
        b.IdUsuario,
        b.IdJuego,
        b.FechaAdquisicion,
        b.HorasJugadas
    );
}