using Milo.Domain.Common;
using Milo.Domain.Common.Interfaces;
using Milo.Domain.Entities.Enums;

namespace Milo.Domain.Entities;

public sealed class Content : BaseEntity, IAuditable, ISoftDeletable
{
    private Content()
    {}
    
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public ContentType Type { get; private set; }
    public bool IsActive { get; private set; }
    
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;

    public void Update(string title, string description, ContentType type)
    {
        Title = title;
        Description = description;
        Type = type;
    }

    public static Content Create(
        string title,
        string description,
        ContentType type) =>
        new()
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Type = type,
            IsActive = true
        };
}