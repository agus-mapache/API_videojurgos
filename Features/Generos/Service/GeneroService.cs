using APIVideoJuegos.Features.Generos.DTOS;
using APIVideoJuegos.Features.Generos.Repository;

namespace APIVideoJuegos.Features.Generos.Service;
public class GeneroService(IGeneroRepository repository) : IGeneroService
{
    public async Task<IEnumerable<GeneroResponseDto>> GetAllAsync()
    {
        var generos = await repository.GetAllAsync();
        return generos.Select(MapToDto);
    }

    public async Task<GeneroResponseDto?> GetByIdAsync(int id)
    {
        var genero = await repository.GetByIdAsync(id);
        return genero is null ? null : MapToDto(genero);
    }

    public async Task<GeneroResponseDto> CreateAsync(CreateGeneroDto dto)
    {
        try
    {
        var genero = new Genero { Nombre = dto.Nombre };
        var id = await repository.CreateAsync(genero);
        return new GeneroResponseDto(id, genero.Nombre);
    }
        catch (MySql.Data.MySqlClient.MySqlException ex) when (ex.Number == 1062)
        {
           
            throw new InvalidOperationException($"El género '{dto.Nombre}' ya existe.");
        }
    }

    public async Task<bool> UpdateAsync(int id, UpdateGeneroDto dto)
    {
        var existente = await repository.GetByIdAsync(id);
        if (existente is null) return false;

        existente.Nombre = dto.Nombre;
        return await repository.UpdateAsync(existente);
    }

    public async Task<bool> DeleteAsync(int id) => await repository.DeleteAsync(id);

    public async Task<IEnumerable<GeneroResponseDto>> GetGenerosByJuegoAsync(int idJuego)
    {
        var generos = await repository.GetGenerosByJuegoIdAsync(idJuego);
        return generos.Select(MapToDto);
    }

    public async Task<bool> AsignarGeneroAJuegoAsync(AsignarGeneroJuegoDto dto)
    {
        return await repository.AsignarGeneroAJuegoAsync(dto.IdJuego, dto.IdGenero);
    }

    public async Task<bool> RemoverGeneroDeJuegoAsync(int idJuego, int idGenero)
    {
        return await repository.RemoverGeneroDeJuegoAsync(idJuego, idGenero);
    }

    private static GeneroResponseDto MapToDto(Genero g) => new(g.IdGenero, g.Nombre);
}