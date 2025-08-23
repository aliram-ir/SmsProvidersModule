using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<SmsProviderEntry> SmsProviderEntries => Set<SmsProviderEntry>();
        public DbSet<SmsLineEntry> SmsLineEntries => Set<SmsLineEntry>();
        public DbSet<SmsTemplate> SmsTemplates => Set<SmsTemplate>();
        public DbSet<SmsConfig> SmsConfigs => Set<SmsConfig>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Auto apply all IEntityTypeConfiguration classes in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
