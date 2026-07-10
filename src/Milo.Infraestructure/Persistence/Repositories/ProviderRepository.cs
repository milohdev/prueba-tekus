using Microsoft.EntityFrameworkCore;
using Milo.Domain.Entities;
using Milo.Domain.Repositories;

namespace Milo.Infraestructure.Persistence.Repositories;

public sealed class ProviderRepository(MiloDbContext dbContext) : IProviderRepository
{
    public async Task<Provider?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Providers.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<(IReadOnlyList<Provider> Items, int TotalCount)> GetPagedAsync(
        string search,
        string sortBy,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Providers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.Name.Contains(search) ||
                p.Nit.Contains(search) ||
                p.Email.Contains(search));
        }

        query = sortBy?.ToLower() switch
        {
            "nit" => query.OrderBy(p => p.Nit),
            "email" => query.OrderBy(p => p.Email),
            _ => query.OrderBy(p => p.Name),
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<bool> ExistByNitAsync(string nit, CancellationToken cancellationToken = default)
        => await dbContext.Providers.AnyAsync(p => p.Nit == nit, cancellationToken);

    public void Add(Provider provider)
        => dbContext.Providers.Add(provider);

    public void Update(Provider provider)
        => dbContext.Providers.Update(provider);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}