using MediatR;
using Milo.Application.Common.Models;

namespace Milo.Application.Service.Queries.GetServices;

public record GetServicesQuery(
    string Search,
    Guid ProviderId,
    string SortBy,
    bool Descending = false,
    int Page = 1,
    int PageSize = 10) : IRequest<Result<PagedResult<ServiceDto>>>;