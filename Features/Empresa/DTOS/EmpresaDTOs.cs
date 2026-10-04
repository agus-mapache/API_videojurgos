namespace APIVideoJuegos.Features.Empresa.DTOS;
public record CreateEmpresaDto(
    string Nombre, 
    string? SitioWeb
);

public record UpdateEmpresaDto(
    string Nombre, 
    string? SitioWeb
);

public record EmpresaResponseDto(
    int IdEmpresa, 
    string Nombre, 
    string? SitioWeb
);