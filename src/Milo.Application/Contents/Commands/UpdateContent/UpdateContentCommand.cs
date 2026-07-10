using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Contents.Queries.GetContents;

namespace Milo.Application.Contents.Commands.UpdateContent;

public record UpdateContentCommand(
    Guid Id,
    string Title,
    string Description,
    string Type) : IRequest<Result<ContentDto>>;