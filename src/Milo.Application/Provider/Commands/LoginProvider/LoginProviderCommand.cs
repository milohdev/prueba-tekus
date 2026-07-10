using MediatR;
using Milo.Application.Common.Models;

namespace Milo.Application.Provider.Commands.LoginProvider;

public record LoginProviderCommand(
    string Email,
    string Password) : IRequest<Result<ProviderAuthResponseDto>>;