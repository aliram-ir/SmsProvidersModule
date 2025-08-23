using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class SmsTemplateConfig : IEntityTypeConfiguration<SmsTemplate>
    {
        public void Configure(EntityTypeBuilder<SmsTemplate> builder)
        {
            builder.ToTable("SmsTemplates");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.TemplateCode)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(t => t.TemplateBody)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(t => t.TemplateType)
                   .HasConversion<int>();

            builder.HasOne(t => t.ProviderEntry)
                   .WithMany(p => p.Templates)
                   .HasForeignKey(t => t.ProviderEntryId);
        }
    }
}
