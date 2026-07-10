using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Service.Queries.GetServices;

namespace Milo.Application.Service.Queries.GetServiceById;

public record GetServiceByIdQuery(Guid Id) : IRequest<Result<ServiceDto>>;