using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class SmsProviderEntryConfig : IEntityTypeConfiguration<SmsProviderEntry>
    {
        public void Configure(EntityTypeBuilder<SmsProviderEntry> builder)
        {
            builder.ToTable("SmsProviderEntries");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProviderType)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(p => p.ApiKey).HasMaxLength(200);
            builder.Property(p => p.Token).HasMaxLength(200);
            builder.Property(p => p.Username).HasMaxLength(100);
            builder.Property(p => p.Password).HasMaxLength(100);

            builder.HasIndex(p => p.IsDefault);
            builder.HasIndex(p => p.IsActive);

            builder.HasMany(p => p.Lines)
                   .WithOne(l => l.ProviderEntry)
                   .HasForeignKey(l => l.ProviderEntryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Templates)
                   .WithOne(t => t.ProviderEntry)
                   .HasForeignKey(t => t.ProviderEntryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
