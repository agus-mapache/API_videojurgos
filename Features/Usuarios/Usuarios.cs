namespace APIVideoJuegos.Features.Usuarios;
public class Usuario
{
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public decimal SaldoBilletera { get; set; }
    public DateTime FechaRegistro { get; set; }

    public string TipoUsuario { get; set; } = "Cliente";
    public int? IdEmpresa { get; set; }
}