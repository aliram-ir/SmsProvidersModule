using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class SmsLineEntryConfig : IEntityTypeConfiguration<SmsLineEntry>
    {
        public void Configure(EntityTypeBuilder<SmsLineEntry> builder)
        {
            builder.ToTable("SmsLineEntries");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.LineNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasIndex(l => l.IsDefault);

            builder.HasOne(l => l.ProviderEntry)
                   .WithMany(p => p.Lines)
                   .HasForeignKey(l => l.ProviderEntryId);
        }
    }
}
