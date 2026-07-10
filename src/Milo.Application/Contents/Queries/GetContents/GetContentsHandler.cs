using MediatR;
using Milo.Application.Common.Models;
using Milo.Domain.Repositories;

namespace Milo.Application.Contents.Queries.GetContents;

public sealed class GetContentsHandler(
    IContentRepository contentRepository) : IRequestHandler<GetContentsQuery, Result<List<ContentDto>>>
{
    public async Task<Result<List<ContentDto>>> Handle(
        GetContentsQuery request, CancellationToken cancellationToken)
    {
        var contents = await contentRepository.GetAllAsync(cancellationToken);

        var dtos = contents.Select(c => new ContentDto(
            c.Id, c.Title, c.Description, c.Type.ToString(), c.IsActive)).ToList();

        return Result<List<ContentDto>>.Success(dtos);
    }
}