using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Contents.Queries.GetContents;
using Milo.Domain.Entities.Enums;
using Milo.Domain.Repositories;

namespace Milo.Application.Contents.Commands.UpdateContent;

public sealed class UpdateContentHandler(
    IContentRepository contentRepository) : IRequestHandler<UpdateContentCommand, Result<ContentDto>>
{
    public async Task<Result<ContentDto>> Handle(
        UpdateContentCommand request, CancellationToken cancellationToken)
    {
        var content = await contentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (content is null)
            return Result<ContentDto>.Failure("Contenido no encontrado");

        var type = Enum.Parse<ContentType>(request.Type, ignoreCase: true);
        content.Update(request.Title, request.Description, type);

        contentRepository.Update(content);
        await contentRepository.SaveChangesAsync(cancellationToken);

        var dto = new ContentDto(
            content.Id, content.Title, content.Description,
            content.Type.ToString(), content.IsActive);

        return Result<ContentDto>.Success(dto);
    }
}