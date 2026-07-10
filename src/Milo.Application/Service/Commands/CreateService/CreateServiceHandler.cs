using MediatR;
using Milo.Application.Common.Interfaces;
using Milo.Application.Common.Models;
using Milo.Application.Service.Queries.GetServices;
using Milo.Domain.Repositories;

namespace Milo.Application.Service.Commands.CreateService;

public sealed class CreateServiceHandler(
    IServiceRepository serviceRepository,
    IProviderRepository providerRepository,
    ICurrentUserProvider currentUserProvider) : IRequestHandler<CreateServiceCommand, Result<ServiceDto>>
{
    public async Task<Result<ServiceDto>> Handle(
        CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var providerId = currentUserProvider.UserId;
        if (providerId is null)
            return Result<ServiceDto>.Failure("No se pudo identificar al proveedor autenticado");

        var provider = await providerRepository.GetByIdAsync(providerId.Value, cancellationToken);
        if (provider is null)
            return Result<ServiceDto>.Failure("Proveedor no encontrado");

        var service = Domain.Entities.Service.Create(
            request.Name, request.CostPerHour, providerId.Value);

        serviceRepository.Add(service);
        await serviceRepository.SaveChangesAsync(cancellationToken);

        var dto = new ServiceDto(
            service.Id, service.Name, service.CostPerHour,
            service.ProviderId, service.IsActive);

        return Result<ServiceDto>.Success(dto);
    }
}