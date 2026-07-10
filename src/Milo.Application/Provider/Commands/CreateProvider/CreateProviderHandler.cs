using MediatR;
using Milo.Application.Common.Interfaces;
using Milo.Application.Common.Models;
using Milo.Application.Provider.Queries.GetProviders;
using Milo.Domain.Entities.Enums;
using Milo.Domain.Repositories;

namespace Milo.Application.Provider.Commands.CreateProvider;

public sealed class CreateProviderHandler(
    IProviderRepository providerRepository,
    IPasswordService passwordService) : IRequestHandler<CreateProviderCommand, Result<ProviderDto>>
{
    public async Task<Result<ProviderDto>> Handle(
        CreateProviderCommand request, CancellationToken cancellationToken)
    {
        if (await providerRepository.ExistByNitAsync(request.Nit, cancellationToken))
            return Result<ProviderDto>.Failure("El NIT ya está registrado");

        var hash = passwordService.Hash(request.Password);
        var provider = Domain.Entities.Provider.Create(
            request.Name, request.Nit, request.PageUrl, request.Email, hash, UserRole.Provider);

        providerRepository.Add(provider);
        await providerRepository.SaveChangesAsync(cancellationToken);

        var dto = new ProviderDto(
            provider.Id, provider.Name, provider.Nit, provider.PageUrl,
            provider.Email, provider.Role.ToString(), provider.IsActive);

        return Result<ProviderDto>.Success(dto);
    }
}