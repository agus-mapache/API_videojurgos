using System.Data;
using Dapper;

namespace APIVideoJuegos.Features.Juegos.Repository;

public class JuegoRepository(IDbConnection connection) : IJuegoRepository
{
    public async Task<IEnumerable<Juego>> GetAllAsync()
    {
        const string sql = "SELECT * FROM juegos";
        return await connection.QueryAsync<Juego>(sql);
    }

    public async Task<Juego?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM juegos WHERE id_juego = @Id";
        return await connection.QuerySingleOrDefaultAsync<Juego>(sql, new { Id = id });
    }

    public async Task<Juego?> GetBySlugAsync(string slug)
    {
        const string sql = "SELECT * FROM juegos WHERE slug = @Slug";
        return await connection.QuerySingleOrDefaultAsync<Juego>(sql, new { Slug = slug });
    }

    public async Task<int> CreateAsync(Juego juego)
    {
        const string sql = """
            INSERT INTO juegos (
                titulo, slug, descripcion, precio_base, 
                descuento_porcentaje, fecha_lanzamiento, 
                id_desarrollador, id_editor
            ) 
            VALUES (
                @Titulo, @Slug, @Descripcion, @PrecioBase, 
                @DescuentoPorcentaje, @FechaLanzamiento, 
                @IdDesarrollador, @IdEditor
            );
            SELECT LAST_INSERT_ID();
            """;
            
        return await connection.ExecuteScalarAsync<int>(sql, juego);
    }

    public async Task<bool> UpdateAsync(Juego juego)
    {
        const string sql = """
            UPDATE juegos 
            SET titulo = @Titulo, 
                slug = @Slug, 
                descripcion = @Descripcion, 
                precio_base = @PrecioBase, 
                descuento_porcentaje = @DescuentoPorcentaje, 
                fecha_lanzamiento = @FechaLanzamiento, 
                id_desarrollador = @IdDesarrollador, 
                id_editor = @IdEditor
            WHERE id_juego = @IdJuego
            """;
            
        var affectedRows = await connection.ExecuteAsync(sql, juego);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM juegos WHERE id_juego = @Id";
        var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }
}