using System.Data;
using Dapper;

namespace APIVideoJuegos.Features.Usuarios.Repository;

public class UsuarioRepository(IDbConnection connection) : IUsuarioRepository
{
    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        const string sql = "SELECT id_usuario AS IdUsuario, nombre_usuario AS NombreUsuario, email, password_hash AS PasswordHash, Fecha_Registro AS FechaRegistro, tipo_usuario AS TipoUsuario, id_empresa AS IdEmpresa FROM usuarios";
        return await connection.QueryAsync<Usuario>(sql);
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT id_usuario AS IdUsuario, nombre_usuario AS NombreUsuario, email, 
                   password_hash AS PasswordHash, fecha_registro AS FechaRegistro, 
                   tipo_usuario AS TipoUsuario, id_empresa AS IdEmpresa 
            FROM usuarios WHERE id_usuario = @Id
            """;
        return await connection.QuerySingleOrDefaultAsync<Usuario>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Usuario usuario)
    {
        const string sql = """
            INSERT INTO usuarios (nombre_usuario, email, password_hash, tipo_usuario, id_empresa) 
            VALUES (@NombreUsuario, @Email, @PasswordHash, @TipoUsuario, @IdEmpresa);
            SELECT LAST_INSERT_ID();
            """;
        return await connection.ExecuteScalarAsync<int>(sql, usuario);
    }

    public async Task<bool> UpdateAsync(Usuario usuario)
    {
        const string sql = """
            UPDATE usuarios 
            SET nombre_usuario = @NombreUsuario, 
                email = @Email, 
                tipo_usuario = @TipoUsuario, 
                id_empresa = @IdEmpresa 
            WHERE id_usuario = @IdUsuario
            """;
        var rows = await connection.ExecuteAsync(sql, usuario);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM usuarios WHERE id_usuario = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}