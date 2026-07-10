using MediatR;
using Milo.Application.Common.Models;
using Milo.Domain.Repositories;

namespace Milo.Application.Service.Commands.DeleteService;

public sealed class DeleteServiceHandler(
    IServiceRepository serviceRepository) : IRequestHandler<DeleteServiceCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await serviceRepository.GetByIdAsync(request.Id, cancellationToken);

        if (service is null)
            return Result<bool>.Failure("Servicio no encontrado");

        serviceRepository.Delete(service);
        await serviceRepository.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}