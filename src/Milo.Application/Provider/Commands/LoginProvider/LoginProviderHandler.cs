using MediatR;
using Milo.Application.Common.Interfaces;
using Milo.Application.Common.Models;
using Milo.Domain.Repositories;

namespace Milo.Application.Provider.Commands.LoginProvider;

public sealed class LoginProviderHandler(
    IProviderRepository providerRepository,
    IPasswordService passwordService,
    IJwtTokenService jwtTokenService) : IRequestHandler<LoginProviderCommand, Result<ProviderAuthResponseDto>>
{
    public async Task<Result<ProviderAuthResponseDto>> Handle(
        LoginProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await providerRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (provider is null || !passwordService.Verify(provider.PasswordHash, request.Password))
            return Result<ProviderAuthResponseDto>.Failure("Credenciales inválidas");

        if (!provider.IsActive)
            return Result<ProviderAuthResponseDto>.Failure("La cuenta está desactivada");

        var (token, expiresAt) = jwtTokenService.GenerateToken(provider);
        return Result<ProviderAuthResponseDto>.Success(new ProviderAuthResponseDto(
            provider.Id, provider.Name, provider.Nit, provider.Email,
            provider.Role.ToString(), token, expiresAt));
    }
}