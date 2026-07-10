using Milo.Domain.Entities;

namespace Milo.Domain.Repositories;

public interface IContentRepository
{
    Task<Content?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Content>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(Content content);
    void Update(Content content);
    void Delete(Content content);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}