using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Provider.Queries.GetProviders;
using Milo.Domain.Repositories;

namespace Milo.Application.Provider.Commands.UpdateProvider;

public sealed class UpdateProviderHandler(
    IProviderRepository providerRepository) : IRequestHandler<UpdateProviderCommand, Result<ProviderDto>>
{
    public async Task<Result<ProviderDto>> Handle(
        UpdateProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await providerRepository.GetByIdAsync(request.Id, cancellationToken);

        if (provider is null)
            return Result<ProviderDto>.Failure("Proveedor no encontrado");

        provider.Update(request.Name, request.PageUrl);

        providerRepository.Update(provider);
        await providerRepository.SaveChangesAsync(cancellationToken);

        var dto = new ProviderDto(
            provider.Id, provider.Name, provider.Nit, provider.PageUrl,
            provider.Email, provider.Role.ToString(), provider.IsActive);

        return Result<ProviderDto>.Success(dto);
    }
}