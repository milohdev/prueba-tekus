using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Contents.Queries.GetContents;
using Milo.Domain.Repositories;

namespace Milo.Application.Contents.Queries.GetContentById;

public sealed class GetContentByIdHandler(
    IContentRepository contentRepository) : IRequestHandler<GetContentByIdQuery, Result<ContentDto>>
{
    public async Task<Result<ContentDto>> Handle(
        GetContentByIdQuery request, CancellationToken cancellationToken)
    {
        var content = await contentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (content is null)
            return Result<ContentDto>.Failure("Contenido no encontrado");

        var dto = new ContentDto(
            content.Id, content.Title, content.Description,
            content.Type.ToString(), content.IsActive);

        return Result<ContentDto>.Success(dto);
    }
}