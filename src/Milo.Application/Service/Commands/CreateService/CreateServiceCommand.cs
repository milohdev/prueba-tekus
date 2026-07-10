using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Service.Queries.GetServices;

namespace Milo.Application.Service.Commands.CreateService;

public record CreateServiceCommand(
    string Name,
    decimal CostPerHour,
    Guid ProviderId) : IRequest<Result<ServiceDto>>;