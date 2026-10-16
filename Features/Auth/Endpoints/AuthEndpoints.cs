namespace APIVideoJuegos.Features.Auth.Endpoints;

using System.Security.Claims;
using APIVideoJuegos.Features.Auth.DTOs;
using APIVideoJuegos.Features.Auth.Service;
using APIVideoJuegos.Features.Usuarios.DTOS;
using APIVideoJuegos.Features.Usuarios.Repository;

using MySql.Data.MySqlClient;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
                       .WithTags("Auth");

        // METHOD: POST
        // ROUTE: /api/auth/signup
        group.MapPost("/signup", async (RegisterRequestDto dto, IAuthService authService) =>
        {
            try
            {
                var resultado = await authService.RegisterAsync(dto);
                return Results.Created($"/api/usuarios/{resultado.Usuario.IdUsuario}", new 
                { 
                    mensaje = "Usuario registrado exitosamente.", 
                    datos = resultado 
                });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { error = ex.Message });
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    return Results.Conflict(new { error = "El nombre de usuario o email ya se encuentra registrado.", detalle = ex.Message });
                }
                if (ex.Number == 1452)
                {
                    return Results.UnprocessableEntity(new { error = "La empresa especificada no existe.", detalle = ex.Message });
                }
                return Results.BadRequest(new { error = "Error en la base de datos al registrar el usuario.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // METHOD: POST
        // ROUTE: /api/auth/login
        group.MapPost("/login", async (LoginRequestDto dto, IAuthService authService) =>
        {
            var resultado = await authService.LoginAsync(dto);
            return resultado is not null
                ? Results.Ok(new { mensaje = "Inicio de sesión exitoso.", datos = resultado })
                : Results.BadRequest(new { error = "Credenciales inválidas. Verifique su email y contraseña." });
        });

        // METHOD: GET
        // ROUTE: /api/auth/me
        group.MapGet("/me", async (ClaimsPrincipal user, IUsuarioRepository usuarioRepo) =>
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out var idUsuario))
            {
                return Results.Unauthorized();
            }

            var usuario = await usuarioRepo.GetByIdAsync(idUsuario);
            if (usuario is null)
            {
                return Results.NotFound(new { mensaje = "Usuario no encontrado." });
            }

            return Results.Ok(new UsuarioResponseDto(
                usuario.IdUsuario,
                usuario.NombreUsuario,
                usuario.Email,
                usuario.TipoUsuario,
                usuario.IdEmpresa,
                usuario.FechaRegistro
            ));
        }).RequireAuthorization();
    }
}
