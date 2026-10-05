using System.Data;
using Dapper;

namespace APIVideoJuegos.Features.Biblioteca.Repository;

public class BibliotecaRepository(IDbConnection connection) : IBibliotecaRepository
{
    public async Task<IEnumerable<BibliotecaItem>> GetByUsuarioIdAsync(int idUsuario)
    {
        const string sql = "SELECT * FROM biblioteca WHERE id_usuario = @IdUsuario";
        return await connection.QueryAsync<BibliotecaItem>(sql, new { IdUsuario = idUsuario });
    }

    public async Task<BibliotecaItem?> GetAsync(int idUsuario, int idJuego)
    {
        const string sql = "SELECT * FROM biblioteca WHERE id_usuario = @IdUsuario AND id_juego = @IdJuego";
        return await connection.QuerySingleOrDefaultAsync<BibliotecaItem>(sql, new { IdUsuario = idUsuario, IdJuego = idJuego });
    }

    public async Task<bool> AddAsync(BibliotecaItem item)
    {
        const string sql = """
            INSERT INTO biblioteca (id_usuario, id_juego, horas_jugadas) 
            VALUES (@IdUsuario, @IdJuego, @HorasJugadas)
            """;
        var rows = await connection.ExecuteAsync(sql, item);
        return rows > 0;
    }

    public async Task<bool> UpdateHorasAsync(int idUsuario, int idJuego, decimal horasJugadas)
    {
        const string sql = """
            UPDATE biblioteca 
            SET horas_jugadas = @HorasJugadas 
            WHERE id_usuario = @IdUsuario AND id_juego = @IdJuego
            """;
        var rows = await connection.ExecuteAsync(sql, new { IdUsuario = idUsuario, IdJuego = idJuego, HorasJugadas = horasJugadas });
        return rows > 0;
    }

    public async Task<bool> RemoveAsync(int idUsuario, int idJuego)
    {
        const string sql = "DELETE FROM biblioteca WHERE id_usuario = @IdUsuario AND id_juego = @IdJuego";
        var rows = await connection.ExecuteAsync(sql, new { IdUsuario = idUsuario, IdJuego = idJuego });
        return rows > 0;
    }
}
