using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Provider.Queries.GetProviders;
using Milo.Domain.Repositories;

namespace Milo.Application.Provider.Queries.GetProviderById;

public sealed class GetProviderByIdHandler(
    IProviderRepository providerRepository) : IRequestHandler<GetProviderByIdQuery, Result<ProviderDto>>
{
    public async Task<Result<ProviderDto>> Handle(
        GetProviderByIdQuery request, CancellationToken cancellationToken)
    {
        var provider = await providerRepository.GetByIdAsync(request.Id, cancellationToken);

        if (provider is null)
            return Result<ProviderDto>.Failure("Proveedor no encontrado");

        var dto = new ProviderDto(
            provider.Id, provider.Name, provider.Nit, provider.PageUrl,
            provider.Email, provider.Role.ToString(), provider.IsActive);

        return Result<ProviderDto>.Success(dto);
    }
}