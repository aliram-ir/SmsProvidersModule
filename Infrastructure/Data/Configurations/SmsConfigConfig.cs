using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class SmsConfigConfig : IEntityTypeConfiguration<SmsConfig>
    {
        public void Configure(EntityTypeBuilder<SmsConfig> builder)
        {
            builder.ToTable("SmsConfig");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedNever();

            builder.Property(x => x.EnableAutoFailoverProvider)
                   .IsRequired();

            builder.Property(x => x.EnableAutoFailoverLineNumber)
                   .IsRequired();

            builder.HasData(new SmsConfig
            {
                Id = 1,
                EnableAutoFailoverProvider = false,
                EnableAutoFailoverLineNumber = false
            });
        }
    }
}
