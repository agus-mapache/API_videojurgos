# Informe de Análisis y Auditoría Integral de la API de Videojuegos

**Fecha:** 16 de Octubre de 2026  
**Proyecto:** API Tienda de Videojuegos (`APIVideoJuegos`)  
**Autor:** Antigravity AI Code Assistant  
**Documento de referencia interna:** [AGENTS.md](file:///d:/proyecto3roanalista/API_videojurgos/AGENTS.md)

---

## 1. Resumen Ejecutivo

Se ha realizado una auditoría exhaustiva de la arquitectura, estructura de directorios, código fuente, modelos de datos, endpoints Minimal APIs, migraciones FluentMigrator y configuración de seguridad del proyecto.

El proyecto cuenta con una base sólida basada en **Vertical Slice Architecture**, separación por capas dentro de cada *feature* y mapeo relacional eficiente mediante **Dapper**. Sin embargo, presenta:
1. **Fallas críticas de seguridad** (contraseñas almacenadas en texto plano, ausencia de endpoint de login, manipulación de precios desde el cliente, autorización rota y ausencia de RBAC).
2. **Lógica de negocio desconectada o incompleta** (compras que no otorgan juegos en la biblioteca, saldo de billetera inoperativo, falta de paginación/filtros en el catálogo).
3. **Inconsistencias arquitectónicas y de convenciones** respecto a los lineamientos estipulados en [AGENTS.md](file:///d:/proyecto3roanalista/API_videojurgos/AGENTS.md).

---

## 2. Estructura y Funcionamiento del Proyecto

### 2.1. Arquitectura General
El proyecto sigue el patrón **Vertical Slice Architecture** bajo `Features/`, dividiendo el dominio en módulos autónomos:
- **`Usuarios`**: Clientes, Desarrolladores y Administradores.
- **`Empresa`**: Estudios desarrolladores y publishers.
- **`Juegos`**: Catálogo de videojuegos con precios y descuentos.
- **`Generos`**: Categorías y tabla intermedia N:M `juego_generos`.
- **`Ordenes`**: Registro de transacciones y detalle de compras (`detalle_orden`).
- **`Biblioteca`**: Licencias adquiridas y registro de horas jugadas.
- **`Resena`**: Reseñas de usuarios por juego.
- **`Migrations`**: Migraciones versionadas con FluentMigrator.

### 2.2. Flujo de Datos
Cada *slice* implementa el siguiente flujo unidireccional:
```text
HTTP Request (Minimal API Endpoints)
         │
         ▼
DTOs (Validation & Transport Records)
         │
         ▼
Service Layer (Business Logic & Mapping)
         │
         ▼
Repository Layer (Dapper SQL Queries)
         │
         ▼
MySQL Database (tienda_videojuegos)
```

---

## 3. Problemas Críticos de Seguridad

A continuación se listan las vulnerabilidades clasificadas por severidad según estándares OWASP / CWE:

### 3.1. [CRÍTICO] Contraseñas Almacenadas en Texto Plano (CWE-256 / CWE-312)
- **Ubicación:** [UsuariosService.cs:L26](file:///d:/proyecto3roanalista/API_videojurgos/Features/Usuarios/Service/UsuariosService.cs#L26)
- **Detalle:** Al registrar un usuario, la contraseña recibida en `CreateUsuarioDto.Password` se asigna directamente a `PasswordHash`:
  ```csharp
  PasswordHash = dto.Password, // ¡Texto plano directo a la BD!
  ```
- **Riesgo:** Si la base de datos se ve comprometida, todas las contraseñas de los usuarios quedan expuestas inmediatamente.
- **Solución requerida:** Implementar un algoritmo de hashing seguro y salado como **BCrypt** (`BCrypt.Net-Next`) o **PBKDF2** / **Argon2**.

### 3.2. [CRÍTICO] Ausencia de Endpoint de Login y Autenticación Rota (OWASP A07:2021)
- **Ubicación:** [JWK/TokenService.cs](file:///d:/proyecto3roanalista/API_videojurgos/JWK/TokenService.cs) y [Program.cs](file:///d:/proyecto3roanalista/API_videojurgos/Program.cs)
- **Detalle:** 
  - La clase `TokenService` está registrada en DI (`builder.Services.AddScoped<TokenService>()`), pero **no existe ningún endpoint HTTP** (`POST /api/auth/login` o similar) que invoque a `GenerarToken`.
  - Múltiples endpoints tienen `.RequireAuthorization()`, pero ningún usuario puede autenticarse legítimamente para obtener un token.
  - Además, `TokenService.GenerarToken` solo incluye el claim `ClaimTypes.Name`, omitiendo el ID del usuario (`NameIdentifier`), Email y su Rol (`ClaimTypes.Role`).

### 3.3. [CRÍTICO] Manipulación de Precios desde el Cliente (Price Tampering - CWE-472 / OWASP A04)
- **Ubicación:** [OrdenDTOS.cs](file:///d:/proyecto3roanalista/API_videojurgos/Features/Ordenes/DTOs/OrdenDTOS.cs#L3-L6) y [OrdenService.cs:L29](file:///d:/proyecto3roanalista/API_videojurgos/Features/Ordenes/Service/OrdenService.cs#L29)
- **Detalle:** El cliente envía el precio que desea pagar en el DTO:
  ```csharp
  public record CreateDetalleOrdenDto(int IdJuego, decimal PrecioComprado);
  ```
  El servicio calcula el total con:
  ```csharp
  var montoTotal = dto.Detalles.Sum(d => d.PrecioComprado);
  ```
  La API **nunca consulta la tabla `juegos`** para validar el precio real ni aplicar el descuento oficial. Un atacante puede enviar `PrecioComprado: 0.00` o incluso valores negativos.

### 3.4. [ALTO] Insecure Direct Object References (IDOR / BOLA - OWASP A01:2021)
- **Ubicación:** Todos los endpoints con parámetros de usuario:
  - `POST /api/ordenes`: Recibe `IdUsuario` en el cuerpo. Cualquier usuario autenticado puede emitir órdenes a nombre de otra persona.
  - `POST /api/biblioteca`: Recibe `IdUsuario`. Permite agregar juegos a la biblioteca de terceros.
  - `PATCH /api/biblioteca/usuario/{idUsuario}/juego/{idJuego}/horas`: Permite alterar las horas jugadas de cualquier usuario.
  - `POST /api/resenas`: Recibe `IdUsuario`. Permite publicar reseñas suplantando identidades.
  - `PUT /api/usuarios/{id}` y `DELETE /api/usuarios/{id}`: No validan que el usuario solicitante sea el propietario de la cuenta o un Administrador.
- **Solución requerida:** Extraer el `IdUsuario` autenticado a partir del `HttpContext.User` (claims del JWT) en lugar de recibirlo en la ruta o cuerpo.

### 3.5. [ALTO] Falta de Control de Acceso Basado en Roles (RBAC - OWASP A01:2021)
- **Ubicación:** Endpoints administrativos en `Juegos`, `Empresas`, `Ordenes`, `Usuarios`.
- **Detalle:** Los endpoints únicamente declaran `.RequireAuthorization()` genérico. No se valida el `TipoUsuario` (`Admin`, `Desarrollador`, `Cliente`).
  - Un cliente regular con un token válido podría:
    - Crear, modificar o eliminar juegos (`POST/PUT/DELETE /api/juegos`).
    - Modificar o eliminar empresas (`POST/PUT/DELETE /api/empresas`).
    - Cambiar el estado de órdenes a "Reembolsado" o "Completado" (`PATCH /api/ordenes/{id}/estado`).
    - Eliminar cuentas de otros usuarios (`DELETE /api/usuarios/{id}`).

### 3.6. [ALTO] Vulnerabilidad de Inyección Arbitraria a Biblioteca (Bypass de Compra)
- **Ubicación:** [BibliotecaEndpoints.cs:L28](file:///d:/proyecto3roanalista/API_videojurgos/Features/Biblioteca/Endpoints/BibliotecaEndpoints.cs#L28)
- **Detalle:** El endpoint `POST /api/biblioteca` permite a cualquier usuario agregar cualquier juego a su biblioteca sin pasar por el flujo de orden ni pagar por él. Debe ser un método interno activado únicamente por la orden completada, o restringido a administradores.

### 3.7. [MEDIO] Claves Secretas y Credenciales Expuestas en `appsettings.json` (CWE-798)
- **Ubicación:** [appsettings.json:L10-L16](file:///d:/proyecto3roanalista/API_videojurgos/appsettings.json#L10-L16)
- **Detalle:** La clave JWT (`Jwt:Key`) y la cadena de conexión a la base de datos con usuario `root` sin contraseña están registradas en texto plano y versionadas en el repositorio Git.
- **Solución requerida:** Utilizar variables de entorno o `User Secrets` (`dotnet user-secrets`) en desarrollo, y Key Vault / Docker Secrets en producción.

### 3.8. [MEDIO] Swagger UI sin Soporte para Probar Endpoints con JWT
- **Ubicación:** [Program.cs:L71](file:///d:/proyecto3roanalista/API_videojurgos/Program.cs#L71)
- **Detalle:** `builder.Services.AddSwaggerGen();` se invoca sin la configuración de `SecurityDefinition` y `SecurityRequirement` de tipo `Bearer`. En Swagger UI no aparece el botón "Authorize", impidiendo ingresar el token JWT para testear los endpoints protegidos.

---

## 4. Fallas en Lógica de Negocio y Características Incompletas

### 4.1. Desconexión entre Órdenes y Biblioteca
- Al crear una orden (`POST /api/ordenes`) o al actualizar su estado a `"Completado"`, los juegos del detalle **nunca se insertan en la tabla `biblioteca`**.
- La compra se procesa de forma aislada, por lo que el usuario paga pero nunca obtiene el juego.

### 4.2. Saldo de Billetera Inerte (`saldo_billetera`)
- La columna `saldo_billetera` existe en la base de datos y en la entidad `Usuario`, pero:
  - No existe endpoint para consultar el saldo de la billetera.
  - No existe endpoint para recargar saldo.
  - Al crear una orden con `MetodoPago: "Saldo Billetera"`, no se verifica si el usuario cuenta con fondos suficientes ni se descuenta el monto de su cuenta.

### 4.3. Bug de Degradación Involuntaria de Usuario en `PUT /api/usuarios/{id}`
- **Ubicación:** [UsuariosDTOS.cs:L16-L17](file:///d:/proyecto3roanalista/API_videojurgos/Features/Usuarios/DTOS/UsuariosDTOS.cs#L16-L17) y [UsuariosService.cs:L50-L51](file:///d:/proyecto3roanalista/API_videojurgos/Features/Usuarios/Service/UsuariosService.cs#L50-L51)
- **Detalle:** `UpdateUsuarioDto` tiene propiedades con `internal set`:
  ```csharp
  public record UpdateUsuarioDto(string NombreUsuario, string Email) {
      public int? IdEmpresa { get; internal set; }
      public string? TipoUsuario { get; internal set; }
  }
  ```
  Al deserializar el JSON del cliente, `TipoUsuario` e `IdEmpresa` quedan en `null`. En `UsuarioService.UpdateAsync`:
  ```csharp
  usuarioExistente.TipoUsuario = string.IsNullOrWhiteSpace(dto.TipoUsuario) ? "Cliente" : dto.TipoUsuario;
  usuarioExistente.IdEmpresa = dto.IdEmpresa;
  ```
  **Efecto:** Si un `Admin` o `Desarrollador` actualiza su nombre o correo, el sistema lo degrada automáticamente a `"Cliente"` y desvincula su empresa (`IdEmpresa = null`).

### 4.4. Ausencia de Validación de Unicidad en Reseñas
- En la tabla `resenas` y su migración [CreateResenasTable.cs](file:///d:/proyecto3roanalista/API_videojurgos/Features/Migrations/CreateResenasTable.cs), no existe una restricción `UNIQUE (id_usuario, id_juego)`.
- Un usuario puede publicar ilimitadas reseñas sobre el mismo juego.
- Tampoco se valida que el usuario realmente posea el juego en su `biblioteca` antes de permitirle publicar una reseña.

### 4.5. Detalle de Orden Retorna `IdDetalle: 0`
- **Ubicación:** [OrdenService.cs:L55](file:///d:/proyecto3roanalista/API_videojurgos/Features/Ordenes/Service/OrdenService.cs#L55)
- **Detalle:** Al crear la orden, el servicio mapea la respuesta con:
  ```csharp
  listaDetalles.Select(d => new DetalleOrdenResponseDto(0, d.IdJuego, d.PrecioComprado)).ToList()
  ```
  Los IDs de los detalles creados se devuelven en `0` en lugar de recuperar los identificadores generados.

### 4.6. Validación de Claves Foráneas Comentada en `JuegoService`
- **Ubicación:** [JuegoService.cs:L40-L43](file:///d:/proyecto3roanalista/API_videojurgos/Features/Juegos/Service/JuegoService.cs#L40-L43)
- **Detalle:** La validación previa para verificar si el desarrollador existe está comentada (`// var desarrolladorExiste = await empresaRepo.GetByIdAsync(...)`), delegando el error enteramente a una excepción de base de datos (`MySqlException 1452`).

---

## 5. Funcionalidades Faltantes en la API

| Módulo | Funcionalidad Faltante | Impacto |
| :--- | :--- | :--- |
| **Auth** | Endpoint de Login (`POST /api/auth/login`) | Bloqueante: Imposibilita el uso del sistema JWT. |
| **Auth** | Endpoint de Registro (`POST /api/auth/register`) | Clave para separar creación pública de gestión administrativa. |
| **Auth** | Endpoint de Perfil (`GET /api/usuarios/me`) | Necesario para que el cliente frontend consulte sus propios datos y saldo. |
| **Usuarios** | Cambio de Contraseña (`PATCH /api/usuarios/me/password`) | Inexistente: Los usuarios no pueden cambiar su clave. |
| **Billetera** | Recarga de Saldo (`POST /api/usuarios/{id}/billetera/recargar`) | Inexistente: La billetera no se puede fondear. |
| **Juegos** | Paginación (`page`, `pageSize`) | Crítico: `GET /api/juegos` trae toda la tabla a memoria. |
| **Juegos** | Búsqueda y Filtros (por título, género, precio, desarrollador) | Inexistente: No hay manera de filtrar el catálogo. |
| **Juegos** | Cálculo de `PrecioFinal` en DTOs | El DTO no calcula el precio con descuento ni incluye géneros asociados. |
| **Biblioteca** | Información completa del juego en listados | `GET /api/biblioteca/usuario/{id}` solo devuelve IDs numéricos sin título ni portada. |
| **Reseñas** | Resumen estadístico (`% Recomendados`, total votos) | No existe endpoint para obtener la calificación general de un juego. |
| **Validación** | Validación de modelos (FluentValidation) | Permite precios negativos, descuentos > 100%, emails con formato inválido, etc. |

---

## 6. Problemas de Configuración, Compilación e Infraestructura

### 6.1. Incompatibilidad de Target Framework (.NET 10 vs SDK Local .NET 9)
- El archivo [APIVideoJuegos.csproj](file:///d:/proyecto3roanalista/API_videojurgos/APIVideoJuegos.csproj) declara:
  ```xml
  <TargetFramework>net10.0</TargetFramework>
  ```
- En el entorno actual está instalado el SDK de .NET 9 (`9.0.201`).
- La ejecución de `dotnet build` falla con el error:
  > `NETSDK1045: El SDK de .NET actual no admite el destino .NET 10.0. Use el destino .NET 9.0 u otro inferior...`
- Si el entorno productivo no dispone de .NET 10 preview, el proyecto debe ajustarse a `net9.0`.

### 6.2. Paquetes de Driver MySQL Redundantes
- El archivo `.csproj` referencia simultáneamente:
  - `MySql.Data` (versión 26.7.0 de Oracle)
  - `MySqlConnector` (versión 2.6.2 de la comunidad)
- Esto introduce dependencias duplicadas e inconsistencias potenciales en el pool de conexiones. Se recomienda estandarizar en `MySqlConnector` por su rendimiento nativo asíncrono.

### 6.3. Manejo de Conexiones en DI y Posible Fuga de Recursos
- En [Program.cs:L74](file:///d:/proyecto3roanalista/API_videojurgos/Program.cs#L74):
  ```csharp
  builder.Services.AddTransient<IDbConnection>(_ => new MySqlConnection(connectionString));
  ```
  La conexión se registra como `Transient`.
- En [OrdenRepository.cs:L114](file:///d:/proyecto3roanalista/API_videojurgos/Features/Ordenes/Repository/OrdenRepository.cs#L114):
  ```csharp
  if (connection.State != ConnectionState.Open)
      connection.Open();
  ```
  La conexión se abre manualmente y nunca se cierra explícitamente (`connection.Close()` o `Dispose()`), confiando en el recolector de basura, lo que puede saturar el pool de MySQL bajo concurrencia.

### 6.4. Incumplimiento de Convenciones de [AGENTS.md](file:///d:/proyecto3roanalista/API_videojurgos/AGENTS.md)
1. **Comentarios obligatorios de rutas:**  
   [AGENTS.md](file:///d:/proyecto3roanalista/API_videojurgos/AGENTS.md) Sección 4 establece:
   ```csharp
   // METHOD: GET
   // ROUTE: /api/entidad/{id}
   group.MapGet("/{id:int}", async (...) => { ... });
   ```
   **Ninguno** de los archivos de endpoints existentes (`UsuarioEndpoints.cs`, `JuegosEndpoints.cs`, `EmpresaEndpoints.cs`, etc.) incluye estos comentarios.
2. **Nomenclatura de carpetas y namespaces:**
   - La carpeta `JWK/` contiene `TokenService.cs` sin namespace y con nombre confuso (JWK en lugar de JWT o Auth).
   - `Features/Usuarios/service/` tiene la `s` minúscula en carpeta y namespace (`APIVideoJuegos.Features.Usuarios.service`), mientras que el resto usa `Service`.
   - Algunas carpetas usan `DTOS` en mayúsculas y otras `DTOs`.

---

## 7. Plan de Acción Recomendado (Roadmap de Solución)

### Fase 1: Seguridad Crítica y Autenticación [IMPLEMENTADA ✅]
1. **Instalación y configuración de BCrypt.Net-Next:**
   - Contraseñas hasheadas de forma segura en `UsuarioService.CreateAsync` con salado automático.
2. **Implementación del Módulo de Autenticación (`Features/Auth`):**
   - Creados DTOs [AuthDTOs.cs](file:///d:/proyecto3roanalista/API_videojurgos/Features/Auth/DTOs/AuthDTOs.cs) (`LoginRequestDto`, `LoginResponseDto`).
   - Implementado [IAuthService.cs](file:///d:/proyecto3roanalista/API_videojurgos/Features/Auth/Service/IAuthService.cs) y [AuthService.cs](file:///d:/proyecto3roanalista/API_videojurgos/Features/Auth/Service/AuthService.cs) validando credenciales contra el hash BCrypt.
   - Refactorizado [TokenService.cs](file:///d:/proyecto3roanalista/API_videojurgos/Features/Auth/Service/TokenService.cs) incorporando claims `NameIdentifier`, `Name`, `Email` y `Role`.
   - Creados endpoints en [AuthEndpoints.cs](file:///d:/proyecto3roanalista/API_videojurgos/Features/Auth/Endpoints/AuthEndpoints.cs): `POST /api/auth/login` y `GET /api/auth/me`.
   - Eliminada la carpeta obsoleta e inconsistente `JWK/`.
3. **Control de Acceso Basado en Roles (RBAC):**
   - Configuradas políticas en `Program.cs`: `AdminOnly`, `DesarrolladorOrAdmin`, `ClienteOrAdmin`.
   - Protegidos los endpoints administrativos y de catálogo según el rol del usuario.
   - Agregados los comentarios de método y ruta (`// METHOD:`, `// ROUTE:`) estipulados en `AGENTS.md`.
4. **Configuración de Swagger con JWT:**
   - Configurado esquema de seguridad `Bearer` y requerimiento en `AddSwaggerGen()`, habilitando el botón "Authorize" en Swagger UI.

### Fase 2: Corrección de Lógica de Negocio y Comercio
1. **Cálculo de Precios del Lado Servidor en Órdenes:**
   - Modificar `CreateOrdenDto` para que solo reciba `List<int> IdJuegos`.
   - En `OrdenService`, consultar los precios reales en la base de datos:
     ```csharp
     var precioFinal = juego.PrecioBase * (1 - (juego.DescuentoPorcentaje / 100m));
     ```
2. **Integración Transaccional con Biblioteca:**
   - Dentro de la misma transacción de creación de la orden (o al completarse), insertar automáticamente los juegos comprados en la tabla `biblioteca`.
3. **Manejo de Billetera:**
   - Si `MetodoPago == "Saldo Billetera"`, verificar `usuario.SaldoBilletera >= total` y descontar el monto dentro de la transacción.
4. **Corregir `UpdateUsuarioDto`:**
   - Modificar el record para no sobreescribir inadvertidamente el `TipoUsuario` ni desvincular el `IdEmpresa`.

### Fase 3: Mejoras del Catálogo y Experiencia de la API
1. **Paginación y Búsqueda en Juegos:**
   - Añadir soporte a `GET /api/juegos?buscar=elden&genero=rpg&pagina=1&tamanio=10`.
2. **Completar DTOs de Juegos:**
   - Devolver `PrecioFinal`, nombres de Desarrollador y Editor, y lista de géneros en `JuegoResponseDto`.
3. **Restricción Unitaria en Reseñas:**
   - Agregar migración FluentMigrator para `UNIQUE (id_usuario, id_juego)` en la tabla `resenas`.
   - Validar que el usuario posea el juego en `biblioteca` antes de permitir reseñar.
4. **Comentarios de Rutas:**
   - Agregar los comentarios `// METHOD:` y `// ROUTE:` exigidos por `AGENTS.md` a todos los endpoints.
5. **Ajuste de Target Framework:**
   - Alinear `.csproj` a `net9.0` (o instalar el SDK de .NET 10 en la máquina host).

---
*Fin del informe.*
