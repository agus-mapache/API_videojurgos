namespace APIVideoJuegos.Features.Empresa.Repository;

public interface IEmpresaRepository
{
    Task<IEnumerable<Empresa>> GetAllAsync();
    Task<Empresa?> GetByIdAsync(int id);
    Task<int> CreateAsync(Empresa empresa);
    Task<bool> UpdateAsync(Empresa empresa);
    Task<bool> DeleteAsync(int id);
}