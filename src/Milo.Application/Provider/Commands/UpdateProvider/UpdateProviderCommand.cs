using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Provider.Queries.GetProviders;

namespace Milo.Application.Provider.Commands.UpdateProvider;

public record UpdateProviderCommand(
    Guid Id,
    string Name,
    string PageUrl) : IRequest<Result<ProviderDto>>;