using APIVideoJuegos.Features.Empresa.DTOS;
using APIVideoJuegos.Features.Empresa.Repository;

namespace APIVideoJuegos.Features.Empresa.Service;

public class EmpresaService(IEmpresaRepository repository) : IEmpresaService
{
    public async Task<IEnumerable<EmpresaResponseDto>> GetAllAsync()
    {
        var empresas = await repository.GetAllAsync();
        return empresas.Select(MapToResponseDto);
    }

    public async Task<EmpresaResponseDto?> GetByIdAsync(int id)
    {
        var empresa = await repository.GetByIdAsync(id);
        return empresa is null ? null : MapToResponseDto(empresa);
    }

    public async Task<EmpresaResponseDto> CreateAsync(CreateEmpresaDto dto)
    {
        var nuevaEmpresa = new Empresa
        {
            Nombre = dto.Nombre,
            SitioWeb = dto.SitioWeb
        };

        var idGenerado = await repository.CreateAsync(nuevaEmpresa);
        
        return new EmpresaResponseDto(idGenerado, nuevaEmpresa.Nombre, nuevaEmpresa.SitioWeb);
    }

    public async Task<bool> UpdateAsync(int id, UpdateEmpresaDto dto)
    {
        var empresaExistente = await repository.GetByIdAsync(id);
        if (empresaExistente is null) return false;

        empresaExistente.Nombre = dto.Nombre;
        empresaExistente.SitioWeb = dto.SitioWeb;

        return await repository.UpdateAsync(empresaExistente);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }

    private static EmpresaResponseDto MapToResponseDto(Empresa e) =>
        new(e.IdEmpresa, e.Nombre, e.SitioWeb);
}