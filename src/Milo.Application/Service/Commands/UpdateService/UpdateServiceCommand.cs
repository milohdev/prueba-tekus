using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Service.Queries.GetServices;

namespace Milo.Application.Service.Commands.UpdateService;

public record UpdateServiceCommand(
    Guid Id,
    string Name,
    decimal CostPerHour) : IRequest<Result<ServiceDto>>;