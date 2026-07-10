using MediatR;
using Milo.Application.Common.Models;

namespace Milo.Application.Service.Commands.DeleteService;

public record DeleteServiceCommand(Guid Id) : IRequest<Result<bool>>;