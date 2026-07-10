using MediatR;
using Milo.Application.Common.Models;

namespace Milo.Application.Provider.Queries.GetProviders;

public record GetProvidersQuery(
    string Search,
    string SortBy,
    int Page = 1,
    int PageSize = 10) : IRequest<Result<PagedResult<ProviderDto>>>;