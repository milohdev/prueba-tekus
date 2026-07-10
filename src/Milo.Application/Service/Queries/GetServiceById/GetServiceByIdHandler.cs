using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Service.Queries.GetServices;
using Milo.Domain.Repositories;

namespace Milo.Application.Service.Queries.GetServiceById;

public sealed class GetServiceByIdHandler(
    IServiceRepository serviceRepository) : IRequestHandler<GetServiceByIdQuery, Result<ServiceDto>>
{
    public async Task<Result<ServiceDto>> Handle(
        GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var service = await serviceRepository.GetByIdAsync(request.Id, cancellationToken);

        if (service is null)
            return Result<ServiceDto>.Failure("Servicio no encontrado");

        var dto = new ServiceDto(
            service.Id, service.Name, service.CostPerHour, service.ProviderId, service.IsActive);

        return Result<ServiceDto>.Success(dto);
    }
}