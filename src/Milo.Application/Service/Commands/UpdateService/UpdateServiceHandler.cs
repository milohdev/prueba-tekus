using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Service.Queries.GetServices;
using Milo.Domain.Repositories;

namespace Milo.Application.Service.Commands.UpdateService;

public sealed class UpdateServiceHandler(
    IServiceRepository serviceRepository) : IRequestHandler<UpdateServiceCommand, Result<ServiceDto>>
{
    public async Task<Result<ServiceDto>> Handle(
        UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await serviceRepository.GetByIdAsync(request.Id, cancellationToken);

        if (service is null)
            return Result<ServiceDto>.Failure("Servicio no encontrado");

        service.Update(request.Name, request.CostPerHour);

        serviceRepository.Update(service);
        await serviceRepository.SaveChangesAsync(cancellationToken);

        var dto = new ServiceDto(
            service.Id, service.Name, service.CostPerHour, service.ProviderId, service.IsActive);

        return Result<ServiceDto>.Success(dto);
    }
}