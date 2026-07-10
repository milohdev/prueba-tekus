using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Provider.Queries.GetProviders;

namespace Milo.Application.Provider.Queries.GetProviderById;

public record GetProviderByIdQuery(Guid Id) : IRequest<Result<ProviderDto>>;