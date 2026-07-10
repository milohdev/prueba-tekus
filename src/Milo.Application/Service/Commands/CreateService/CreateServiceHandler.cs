using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Service.Queries.GetServices;
using Milo.Domain.Repositories;

namespace Milo.Application.Service.Commands.CreateService;

public sealed class CreateServiceHandler(
    IServiceRepository serviceRepository,
    IProviderRepository providerRepository) : IRequestHandler<CreateServiceCommand, Result<ServiceDto>>
{
    public async Task<Result<ServiceDto>> Handle(
        CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var provider = await providerRepository.GetByIdAsync(request.ProviderId, cancellationToken);
        if (provider is null)
            return Result<ServiceDto>.Failure("Proveedor no encontrado");

        var service = Domain.Entities.Service.Create(
            request.Name, request.CostPerHour, request.ProviderId);

        serviceRepository.Add(service);
        await serviceRepository.SaveChangesAsync(cancellationToken);

        var dto = new ServiceDto(
            service.Id, service.Name, service.CostPerHour,
            service.ProviderId, service.IsActive);

        return Result<ServiceDto>.Success(dto);
    }
}