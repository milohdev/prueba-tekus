using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Provider.Queries.GetProviders;

namespace Milo.Application.Provider.Commands.CreateProvider;

public record CreateProviderCommand(
    string Name,
    string Nit,
    string PageUrl,
    string Email,
    string Password) : IRequest<Result<ProviderDto>>;