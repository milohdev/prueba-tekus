using MediatR;
using Milo.Application.Common.Models;

namespace Milo.Application.Contents.Commands.DeleteContent;

public record DeleteContentCommand(Guid Id) : IRequest<Result<bool>>;