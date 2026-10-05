# Guía y Convenciones del Proyecto (AGENTS.md)

Este documento describe la arquitectura, convenciones de código, base de datos y estándares de diseño de la **API de Tienda de Videojuegos**. Sirve como referencia técnica para el desarrollo y mantenimiento del proyecto.

---

## 1. Stack Tecnológico

- **Framework:** .NET 10 (C#) - ASP.NET Core Minimal APIs.
- **ORM / Micro-ORM:** Dapper (con `Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;`).
- **Base de Datos:** MySQL / MariaDB (Driver `MySql.Data.MySqlClient`).
- **Documentación API:** Swagger / OpenAPI (Swashbuckle).

---

## 2. Estructura del Proyecto (Vertical Slice Architecture)

El proyecto está organizado por **Features (Características)** dentro de la carpeta `Features/`. Cada feature es autónoma y contiene sus propias capas:

```text
Features/
  ├── [NombreFeature]/
  │     ├── [NombreFeature].cs            # Entidad / Modelo de dominio
  │     ├── DTOs/
  │     │     └── [NombreFeature]DTOs.cs   # Records para Create, Update y Response
  │     ├── Repository/
  │     │     ├── I[NombreFeature]Repository.cs
  │     │     └── [NombreFeature]Repository.cs # Consultas SQL puras con Dapper
  │     ├── Service/
  │     │     ├── I[NombreFeature]Service.cs
  │     │     └── [NombreFeature]Service.cs    # Lógica de negocio y mapeo a DTOs
  │     └── Endpoints/
  │           └── [NombreFeature]Endpoints.cs  # Mapeo de rutas Minimal API
```

### Features actuales:
1. **`Empresa`**: Gestión de desarrolladoras y publishers.
2. **`Usuarios`**: Clientes, Desarrolladores y Administradores.
3. **`Juegos`**: Catálogo de videojuegos con precios, descuentos y relaciones.
4. **`Generos`**: Categorías y relaciones N:M con juegos (`juego_generos`).
5. **`Ordenes`**: Compras, transacciones y detalle de órdenes (`detalle_orden`).
6. **`Biblioteca`**: Juegos adquiridos y seguimiento de horas jugadas por usuario.
7. **`Resena`**: Reseñas y recomendaciones por juego/usuario (sin listado general `GET /`).

---

## 3. Convenciones de Base de Datos y Mapeo Dapper

- **Script de creación:** [DATABASE.sql](file:///c:/dev/Hilet/API_videojurgos/DATABASE.sql).
- **Convención de nombres en BD:** `snake_case` (ej. `id_usuario`, `nombre_usuario`, `fecha_registro`, `sitio_web`).
- **Convención de nombres en C#:** `PascalCase` (ej. `IdUsuario`, `NombreUsuario`, `FechaRegistro`, `SitioWeb`).
- **Mapeo Automático:** Se inicializa en `Program.cs` mediante:
  ```csharp
  Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
  ```
  Esto mapea automáticamente columnas `snake_case` a propiedades `PascalCase` sin requerir alias manuales en SQL (`SELECT * FROM ...`).
- **Claves Primarias:** Todas las tablas principales usan `AUTO_INCREMENT`. Los `INSERT` no deben incluir los IDs primarios para permitir la autogeneración correlativa.

---

## 4. Estándar de Endpoints y Documentación

### Formato de Comentarios en Rutas:
Cada endpoint **debe** incluir obligatoriamente el comentario de método y ruta encima de su definición:

```csharp
// METHOD: GET
// ROUTE: /api/entidad/{id}
group.MapGet("/{id:int}", async (...) => { ... });
```

### Respuestas y Feedback Uniforme:
- **`GET (Listado)`**: `Results.Ok(lista)`
- **`GET (Por Id/Slug)`**: `Results.Ok(item)` si existe, o `Results.NotFound(new { mensaje = "..." })` si no se encuentra.
- **`POST (Creación)`**: `Results.Created($"/api/.../{id}", new { mensaje = "...", datos = creado })`.
- **`PUT / PATCH (Actualización)`**: `Results.Ok(new { mensaje = "..." })` si se actualizó, o `Results.NotFound(new { mensaje = "..." })`.
- **`DELETE (Eliminación)`**: `Results.Ok(new { mensaje = "..." })` si se eliminó, o `Results.NotFound(new { mensaje = "..." })`.

### Manejo de Excepciones:
Los endpoints de modificación (`POST`, `PUT`, `PATCH`, `DELETE`) deben capturar:
- `InvalidOperationException`: Errores de validación de negocio $\rightarrow$ `Results.BadRequest(new { error = ex.Message })`.
- `MySqlException`:
  - Código `1062` (Duplicado): `Results.Conflict(new { error = "...", detalle = ex.Message })`.
  - Código `1452` (Clave foránea no encontrada): `Results.BadRequest(new { error = "El recurso relacionado no existe.", detalle = ex.Message })`.
  - General: `Results.BadRequest(new { error = "Error en la base de datos.", detalle = ex.Message })`.
- `Exception`: `Results.BadRequest(new { error = ex.Message })`.

---

## 5. Comandos Frecuentes

```bash
# Ejecutar en modo desarrollo con hot-reload
dotnet watch run dev

# Compilar proyecto
dotnet build

# Ejecutar proyecto normal
dotnet run
```
