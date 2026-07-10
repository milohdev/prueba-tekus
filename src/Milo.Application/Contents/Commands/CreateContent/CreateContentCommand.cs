using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Contents.Queries.GetContents;

namespace Milo.Application.Contents.Commands.CreateContent;

public record CreateContentCommand(
    string Title,
    string Description,
    string Type) : IRequest<Result<ContentDto>>;