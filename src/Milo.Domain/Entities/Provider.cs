using Milo.Domain.Common;
using Milo.Domain.Common.Interfaces;
using Milo.Domain.Entities.Enums;

namespace Milo.Domain.Entities;

public sealed class Provider : BaseEntity, IAuditable, ISoftDeletable
{
    private Provider()
    {}
    
    
    public string Nit { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string PageUrl { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public UserRole Role { get; private set; }
    
    public ICollection<Service> Services { get; } = new List<Service>();
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }



    public static Provider Create(
        string name, string nit, string pageUrl, string email, string passwordHash, UserRole role
    ) => new()
    {
        Id =  Guid.NewGuid(),
        Name = name,
        Nit = nit,
        PageUrl = pageUrl,
        Email = email,
        Role = role,
        PasswordHash =  passwordHash,
        IsActive = true,
    };


}