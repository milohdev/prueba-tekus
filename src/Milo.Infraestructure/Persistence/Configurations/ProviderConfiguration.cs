using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Milo.Domain.Entities;

namespace Milo.Infraestructure.Persistence.Configurations;

public sealed class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.Nit).IsUnique();
        builder.Property(p => p.Nit).HasMaxLength(20).IsRequired();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.PageUrl).HasMaxLength(500).IsRequired();
        builder.Property(p => p.Email).HasMaxLength(200).IsRequired();
        
        builder.HasMany(p => p.Services)
            .WithOne(s => s.Provider)
            .HasForeignKey(s => s.ProviderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
}