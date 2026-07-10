using Milo.Domain.Entities;

namespace Milo.Domain.Repositories;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Service> Items, int TotalCount)> GetPagedAsync(
        string search,
        Guid providerId,
        string sortBy,
        bool descending,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    void Add(Service service);
    void Update(Service service);
    void Delete(Service service);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    
}