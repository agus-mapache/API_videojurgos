using APIVideoJuegos.Features.Empresa.DTOS;

namespace APIVideoJuegos.Features.Empresa.Service;

public interface IEmpresaService
{
    Task<IEnumerable<EmpresaResponseDto>> GetAllAsync();
    Task<EmpresaResponseDto?> GetByIdAsync(int id);
    Task<EmpresaResponseDto> CreateAsync(CreateEmpresaDto dto);
    Task<bool> UpdateAsync(int id, UpdateEmpresaDto dto);
    Task<bool> DeleteAsync(int id);
}