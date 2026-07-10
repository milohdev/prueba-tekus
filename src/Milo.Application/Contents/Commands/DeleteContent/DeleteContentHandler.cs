using MediatR;
using Milo.Application.Common.Models;
using Milo.Domain.Repositories;

namespace Milo.Application.Contents.Commands.DeleteContent;

public sealed class DeleteContentHandler(
    IContentRepository contentRepository) : IRequestHandler<DeleteContentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeleteContentCommand request, CancellationToken cancellationToken)
    {
        var content = await contentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (content is null)
            return Result<bool>.Failure("Contenido no encontrado");

        contentRepository.Delete(content);
        await contentRepository.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}