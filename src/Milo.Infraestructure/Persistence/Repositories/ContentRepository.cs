using Microsoft.EntityFrameworkCore;
using Milo.Domain.Entities;
using Milo.Domain.Repositories;
using Milo.Infraestructure.Persistence;

namespace Milo.Infraestructure.Persistence.Repositories;

public sealed class ContentRepository (MiloDbContext dbContext) : IContentRepository
{
    public async Task<Content?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Contents.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<List<Content>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.Contents.ToListAsync(cancellationToken);

    public void Add(Content content)
        => dbContext.Contents.Add(content);

    public void Update(Content content)
        => dbContext.Contents.Update(content);

    public void Delete(Content content)
        => dbContext.Contents.Remove(content);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
    
}