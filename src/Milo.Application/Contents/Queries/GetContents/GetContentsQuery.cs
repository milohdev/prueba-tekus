using MediatR;
using Milo.Application.Common.Models;

namespace Milo.Application.Contents.Queries.GetContents;

public record GetContentsQuery : IRequest<Result<List<ContentDto>>>;