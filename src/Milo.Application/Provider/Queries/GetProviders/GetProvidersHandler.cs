using MediatR;
using Milo.Application.Common.Models;
using Milo.Domain.Repositories;

namespace Milo.Application.Provider.Queries.GetProviders;

public sealed class GetProvidersHandler(
    IProviderRepository providerRepository)
    : IRequestHandler<GetProvidersQuery, Result<PagedResult<ProviderDto>>>
{
    public async Task<Result<PagedResult<ProviderDto>>> Handle(
        GetProvidersQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await providerRepository.GetPagedAsync(
            request.Search, request.SortBy, request.Page, request.PageSize, cancellationToken);

        var dtos = items.Select(p => new ProviderDto(
            p.Id, p.Name, p.Nit, p.PageUrl, p.Email, p.Role.ToString(), p.IsActive)).ToList();

        var result = new PagedResult<ProviderDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
        };

        return Result<PagedResult<ProviderDto>>.Success(result);
    }
}