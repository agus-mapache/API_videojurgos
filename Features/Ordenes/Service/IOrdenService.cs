using APIVideoJuegos.Features.Ordenes.DTOs;

namespace APIVideoJuegos.Features.Ordenes.Service;

public interface IOrdenService
{
    Task<IEnumerable<OrdenResponseDto>> GetAllAsync();
    Task<OrdenResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<OrdenResponseDto>> GetByUsuarioAsync(int idUsuario);
    Task<OrdenResponseDto> CreateAsync(CreateOrdenDto dto);
    Task<bool> UpdateEstadoAsync(int idOrden, string nuevoEstado);
    Task<bool> DeleteAsync(int id);
}