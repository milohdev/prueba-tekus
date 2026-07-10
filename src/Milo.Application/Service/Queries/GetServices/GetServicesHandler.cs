using MediatR;
using Milo.Application.Common.Models;
using Milo.Domain.Repositories;

namespace Milo.Application.Service.Queries.GetServices;

public sealed class GetServicesHandler(
    IServiceRepository serviceRepository)
    : IRequestHandler<GetServicesQuery, Result<PagedResult<ServiceDto>>>
{
    public async Task<Result<PagedResult<ServiceDto>>> Handle(
        GetServicesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await serviceRepository.GetPagedAsync(
            request.Search, request.ProviderId, request.SortBy,
            request.Descending, request.Page, request.PageSize, cancellationToken);

        var dtos = items.Select(s => new ServiceDto(
            s.Id, s.Name, s.CostPerHour, s.ProviderId, s.IsActive)).ToList();

        var result = new PagedResult<ServiceDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
        };

        return Result<PagedResult<ServiceDto>>.Success(result);
    }
}