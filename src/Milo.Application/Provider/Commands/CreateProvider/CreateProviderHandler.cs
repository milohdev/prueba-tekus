using MediatR;
using Milo.Application.Common.Interfaces;
using Milo.Application.Common.Models;
using Milo.Domain.Entities.Enums;
using Milo.Domain.Repositories;

namespace Milo.Application.Provider.Commands.CreateProvider;

public sealed class CreateProviderHandler(
    IProviderRepository providerRepository,
    IPasswordService passwordService,
    IJwtTokenService jwtTokenService) : IRequestHandler<CreateProviderCommand, Result<ProviderAuthResponseDto>>
{
    public async Task<Result<ProviderAuthResponseDto>> Handle(
        CreateProviderCommand request, CancellationToken cancellationToken)
    {
        if (await providerRepository.ExistByNitAsync(request.Nit, cancellationToken))
            return Result<ProviderAuthResponseDto>.Failure("El NIT ya está registrado");

        var hash = passwordService.Hash(request.Password);
        var provider = Domain.Entities.Provider.Create(
            request.Name, request.Nit, request.PageUrl, request.Email, hash, UserRole.Provider);

        providerRepository.Add(provider);
        await providerRepository.SaveChangesAsync(cancellationToken);

        var (token, expiresAt) = jwtTokenService.GenerateToken(provider);
        return Result<ProviderAuthResponseDto>.Success(new ProviderAuthResponseDto(
            provider.Id, provider.Name, provider.Nit, provider.Email,
            provider.Role.ToString(), token, expiresAt));
    }
}