namespace APIVideoJuegos.Features.Empresa;
public class Empresa
{
    public int IdEmpresa { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? SitioWeb { get; set; } // Es nullable porque no es NOT NULL en la BD
}