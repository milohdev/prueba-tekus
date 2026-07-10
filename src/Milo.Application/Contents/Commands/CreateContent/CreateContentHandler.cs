using MediatR;
using Milo.Application.Common.Models;
using Milo.Application.Contents.Queries.GetContents;
using Milo.Domain.Entities;
using Milo.Domain.Entities.Enums;
using Milo.Domain.Repositories;

namespace Milo.Application.Contents.Commands.CreateContent;

public sealed class CreateContentHandler(
    IContentRepository contentRepository) : IRequestHandler<CreateContentCommand, Result<ContentDto>>
{
    public async Task<Result<ContentDto>> Handle(
        CreateContentCommand request, CancellationToken cancellationToken)
    {
        var type = Enum.Parse<ContentType>(request.Type, ignoreCase: true);
        var content = Content.Create(request.Title, request.Description, type);

        contentRepository.Add(content);
        await contentRepository.SaveChangesAsync(cancellationToken);

        var dto = new ContentDto(
            content.Id, content.Title, content.Description,
            content.Type.ToString(), content.IsActive);

        return Result<ContentDto>.Success(dto);
    }
}