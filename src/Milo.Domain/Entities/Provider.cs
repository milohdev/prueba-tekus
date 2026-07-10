using Milo.Domain.Common;
using Milo.Domain.Common.Interfaces;
using Milo.Domain.Entities.Enums;

namespace Milo.Domain.Entities;

public sealed class Provider : BaseEntity, IAuditable, ISoftDeletable
{
    private Provider()
    {}
    
    
    public string Nit { get; private set; } 
    public string Name { get; private set; }
    public string PageUrl { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    
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