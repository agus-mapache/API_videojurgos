using System.Data;
using Dapper;

namespace APIVideoJuegos.Features.Resena.Repository;

public class ResenaRepository(IDbConnection connection) : IResenaRepository
{
    public async Task<IEnumerable<Resena>> GetByJuegoIdAsync(int idJuego)
    {
        const string sql = "SELECT * FROM resenas WHERE id_juego = @IdJuego";
        return await connection.QueryAsync<Resena>(sql, new { IdJuego = idJuego });
    }

    public async Task<IEnumerable<Resena>> GetByUsuarioIdAsync(int idUsuario)
    {
        const string sql = "SELECT * FROM resenas WHERE id_usuario = @IdUsuario";
        return await connection.QueryAsync<Resena>(sql, new { IdUsuario = idUsuario });
    }

    public async Task<Resena?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM resenas WHERE id_resena = @Id";
        return await connection.QuerySingleOrDefaultAsync<Resena>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Resena resena)
    {
        const string sql = """
            INSERT INTO resenas (id_usuario, id_juego, recomendado, comentario) 
            VALUES (@IdUsuario, @IdJuego, @Recomendado, @Comentario);
            SELECT LAST_INSERT_ID();
            """;
        return await connection.ExecuteScalarAsync<int>(sql, resena);
    }

    public async Task<bool> UpdateAsync(Resena resena)
    {
        const string sql = """
            UPDATE resenas 
            SET recomendado = @Recomendado, 
                comentario = @Comentario 
            WHERE id_resena = @IdResena
            """;
        var rows = await connection.ExecuteAsync(sql, resena);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM resenas WHERE id_resena = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
