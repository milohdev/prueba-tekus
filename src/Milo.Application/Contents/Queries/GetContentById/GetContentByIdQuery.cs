using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Contents.Queries.GetContents;

namespace Milo.Application.Contents.Queries.GetContentById;

public record GetContentByIdQuery(Guid Id) : IRequest<Result<ContentDto>>;