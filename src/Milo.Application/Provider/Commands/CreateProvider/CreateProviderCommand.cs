using MediatR;
using Milo.Application.Common.Models;

namespace Milo.Application.Provider.Commands.CreateProvider;

public record CreateProviderCommand(
    string Name,
    string Nit,
    string PageUrl,
    string Email,
    string Password) : IRequest<Result<ProviderAuthResponseDto>>;