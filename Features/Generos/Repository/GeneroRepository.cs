using System.Data;
using Dapper;

namespace APIVideoJuegos.Features.Generos.Repository;

public class GeneroRepository(IDbConnection connection) : IGeneroRepository
{
    public async Task<IEnumerable<Genero>> GetAllAsync()
    {
        const string sql = "SELECT * FROM generos";
        return await connection.QueryAsync<Genero>(sql);
    }

    public async Task<Genero?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM generos WHERE id_genero = @Id";
        return await connection.QuerySingleOrDefaultAsync<Genero>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Genero genero)
    {
        const string sql = """
            INSERT INTO generos (nombre) VALUES (@Nombre);
            SELECT LAST_INSERT_ID();
            """;
        return await connection.ExecuteScalarAsync<int>(sql, genero);
    }

    public async Task<bool> UpdateAsync(Genero genero)
    {
        const string sql = "UPDATE generos SET nombre = @Nombre WHERE id_genero = @IdGenero";
        var rows = await connection.ExecuteAsync(sql, genero);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM generos WHERE id_genero = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<IEnumerable<Genero>> GetGenerosByJuegoIdAsync(int idJuego)
    {
        const string sql = """
            SELECT g.* FROM generos g
            INNER JOIN juego_generos jg ON g.id_genero = jg.id_genero
            WHERE jg.id_juego = @IdJuego
            """;
        return await connection.QueryAsync<Genero>(sql, new { IdJuego = idJuego });
    }

    public async Task<bool> AsignarGeneroAJuegoAsync(int idJuego, int idGenero)
    {
        const string sql = """
            INSERT IGNORE INTO juego_generos (id_juego, id_genero) 
            VALUES (@IdJuego, @IdGenero)
            """;
        var rows = await connection.ExecuteAsync(sql, new { IdJuego = idJuego, IdGenero = idGenero });
        return rows > 0;
    }

    public async Task<bool> RemoverGeneroDeJuegoAsync(int idJuego, int idGenero)
    {
        const string sql = "DELETE FROM juego_generos WHERE id_juego = @IdJuego AND id_genero = @IdGenero";
        var rows = await connection.ExecuteAsync(sql, new { IdJuego = idJuego, IdGenero = idGenero });
        return rows > 0;
    }
}