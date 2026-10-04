using System.Data;
using Dapper;

namespace APIVideoJuegos.Features.Empresa.Repository;

public class EmpresaRepository(IDbConnection connection) : IEmpresaRepository
{
    public async Task<IEnumerable<Empresa>> GetAllAsync()
    {
        const string sql = "SELECT * FROM empresas";
        return await connection.QueryAsync<Empresa>(sql);
    }

    public async Task<Empresa?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM empresas WHERE id_empresa = @Id";
        return await connection.QuerySingleOrDefaultAsync<Empresa>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Empresa empresa)
    {
        const string sql = """
            INSERT INTO empresas (nombre, sitio_web) 
            VALUES (@Nombre, @SitioWeb);
            SELECT LAST_INSERT_ID();
            """;
            
        return await connection.ExecuteScalarAsync<int>(sql, empresa);
    }

    public async Task<bool> UpdateAsync(Empresa empresa)
    {
        const string sql = """
            UPDATE empresas 
            SET nombre = @Nombre, 
                sitio_web = @SitioWeb 
            WHERE id_empresa = @IdEmpresa
            """;
            
        var affectedRows = await connection.ExecuteAsync(sql, empresa);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM empresas WHERE id_empresa = @Id";
        var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }
}