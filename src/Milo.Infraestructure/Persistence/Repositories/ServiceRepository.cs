using Microsoft.EntityFrameworkCore;
using Milo.Domain.Entities;
using Milo.Domain.Repositories;

namespace Milo.Infraestructure.Persistence.Repositories;

public sealed class ServiceRepository(MiloDbContext dbContext) : IServiceRepository
{
    public async Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Services.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<(IReadOnlyList<Service> Items, int TotalCount)> GetPagedAsync(
        string search,
        Guid providerId,
        string sortBy,
        bool descending,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Services.AsQueryable();

        if (providerId != Guid.Empty)
        {
            query = query.Where(s => s.ProviderId == providerId);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s => s.Name.Contains(search));
        }

        query = sortBy?.ToLower() switch
        {
            "costperhour" => descending
                ? query.OrderByDescending(s => s.CostPerHour)
                : query.OrderBy(s => s.CostPerHour),
            _ => descending
                ? query.OrderByDescending(s => s.Name)
                : query.OrderBy(s => s.Name),
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public void Add(Service service)
        => dbContext.Services.Add(service);

    public void Update(Service service)
        => dbContext.Services.Update(service);

    public void Delete(Service service)
        => dbContext.Services.Remove(service);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}