using System.Data;
using Dapper;

namespace APIVideoJuegos.Features.Ordenes.Repository;
public class OrdenRepository(IDbConnection connection) : IOrdenRepository
{
    public async Task<IEnumerable<Orden>> GetAllAsync()
    {
        const string sql = """
            SELECT o.*, d.* 
            FROM ordenes o
            LEFT JOIN detalle_orden d ON o.id_orden = d.id_orden
            """;

        var ordenesDict = new Dictionary<int, Orden>();

        await connection.QueryAsync<Orden, DetalleOrden, Orden>(
            sql,
            (orden, detalle) =>
            {
                if (!ordenesDict.TryGetValue(orden.IdOrden, out var currentOrden))
                {
                    currentOrden = orden;
                    currentOrden.Detalles = [];
                    ordenesDict.Add(currentOrden.IdOrden, currentOrden);
                }

                if (detalle != null)
                {
                    currentOrden.Detalles.Add(detalle);
                }

                return currentOrden;
            },
            splitOn: "id_detalle"
        );

        return ordenesDict.Values;
    }

    public async Task<Orden?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT o.*, d.* 
            FROM ordenes o
            LEFT JOIN detalle_orden d ON o.id_orden = d.id_orden
            WHERE o.id_orden = @Id
            """;

        Orden? ordenResult = null;
        var detallesList = new List<DetalleOrden>();

        await connection.QueryAsync<Orden, DetalleOrden, Orden>(
            sql,
            (orden, detalle) =>
            {
                ordenResult ??= orden;
                if (detalle != null)
                {
                    detallesList.Add(detalle);
                }
                return orden;
            },
            new { Id = id },
            splitOn: "id_detalle"
        );

        if (ordenResult != null)
        {
            ordenResult.Detalles = detallesList;
        }

        return ordenResult;
    }

    public async Task<IEnumerable<Orden>> GetByUsuarioIdAsync(int idUsuario)
    {
        const string sql = """
            SELECT o.*, d.* 
            FROM ordenes o
            LEFT JOIN detalle_orden d ON o.id_orden = d.id_orden
            WHERE o.id_usuario = @IdUsuario
            """;

        var ordenesDict = new Dictionary<int, Orden>();

        await connection.QueryAsync<Orden, DetalleOrden, Orden>(
            sql,
            (orden, detalle) =>
            {
                if (!ordenesDict.TryGetValue(orden.IdOrden, out var currentOrden))
                {
                    currentOrden = orden;
                    currentOrden.Detalles = [];
                    ordenesDict.Add(currentOrden.IdOrden, currentOrden);
                }

                if (detalle != null)
                {
                    currentOrden.Detalles.Add(detalle);
                }

                return currentOrden;
            },
            new { IdUsuario = idUsuario },
            splitOn: "id_detalle"
        );

        return ordenesDict.Values;
    }

    public async Task<int> CreateAsync(Orden orden, IEnumerable<DetalleOrden> detalles)
    {
        if (connection.State != ConnectionState.Open)
            connection.Open();

        using var transaction = connection.BeginTransaction();
        try
        {
            // 1. Insertar Cabecera de la Orden
            const string sqlOrden = """
                INSERT INTO ordenes (id_usuario, monto_total, estado, metodo_pago) 
                VALUES (@IdUsuario, @MontoTotal, @Estado, @MetodoPago);
                SELECT LAST_INSERT_ID();
                """;

            var idOrden = await connection.ExecuteScalarAsync<int>(sqlOrden, orden, transaction);

            // 2. Insertar los detalles vinculados a la orden creada
            const string sqlDetalle = """
                INSERT INTO detalle_orden (id_orden, id_juego, precio_comprado) 
                VALUES (@IdOrden, @IdJuego, @PrecioComprado)
                """;

            foreach (var detalle in detalles)
            {
                detalle.IdOrden = idOrden;
                await connection.ExecuteAsync(sqlDetalle, detalle, transaction);
            }

            transaction.Commit();
            return idOrden;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> UpdateEstadoAsync(int idOrden, string nuevoEstado)
    {
        const string sql = "UPDATE ordenes SET estado = @Estado WHERE id_orden = @IdOrden";
        var rows = await connection.ExecuteAsync(sql, new { Estado = nuevoEstado, IdOrden = idOrden });
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // Gracias a ON DELETE CASCADE en la tabla detalle_orden, eliminar la orden borra sus detalles automáticamente.
        const string sql = "DELETE FROM ordenes WHERE id_orden = @Id";
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}