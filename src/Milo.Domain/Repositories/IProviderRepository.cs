using Milo.Domain.Entities;

namespace Milo.Domain.Repositories;

public interface IProviderRepository
{
    Task<Provider?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Provider?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Provider> Items, int TotalCount)> GetPagedAsync(
        string search,
        string sortBy,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<bool>  ExistByNitAsync(string nit, CancellationToken cancellationToken = default);
    void Add(Provider provider);
    void Update(Provider provider);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    
}


