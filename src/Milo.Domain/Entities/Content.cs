using System.Net.Mime;
using Milo.Domain.Common;
using Milo.Domain.Common.Interfaces;
using Milo.Domain.Entities.Enums;

namespace Milo.Domain.Entities;

public sealed class ContentType : BaseEntity, IAuditable, ISoftDeletable
{
    private ContentType()
    {}
    
    public string Title { get; private set; }
    public string Description { get; private set; }
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

    public static ContentType Create(
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