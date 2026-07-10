using Milo.Domain.Common;
using Milo.Domain.Common.Interfaces;

namespace Milo.Domain.Entities;

public sealed class Service : BaseEntity, IAuditable, ISoftDeletable
{
    private Service()
    {}
    
    public string Name { get; private set; } = default!;
    public decimal CostPerHour { get; private set; }

    public Guid ProviderId { get; private set; }

    public bool IsActive { get; private set; }
    public Provider Provider { get; private set; } = default!;
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    

    public static Service Create(
        string name, decimal costPerHour, Guid providerId
    ) => new()
    {
        Id =  Guid.NewGuid(),
        Name = name,
        CostPerHour = costPerHour,
        IsActive = true,
        ProviderId = providerId
   
    };
    
    public void Update(string name, decimal costPerHour)
    {
        Name = name;
        CostPerHour = costPerHour;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
    
    
}