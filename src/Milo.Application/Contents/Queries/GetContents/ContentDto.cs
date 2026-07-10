namespace Milo.Application.Contents.Queries.GetContents;

public record ContentDto(
    Guid Id,
    string Title,
    string Description,
    string Type,
    bool IsActive);