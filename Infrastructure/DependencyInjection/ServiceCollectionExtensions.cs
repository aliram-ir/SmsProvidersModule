using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MyApiDb")));

            services.AddMemoryCache();

            // توجه: الان IGenericRepository از طریق UnitOfWork ساخته میشه
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // سرویس‌های SMS
            services.AddScoped<ISmsProviderSelector, SmsProviderSelector>();
            services.AddScoped<ISmsService, SmsService>();

            return services;
        }
    }
}
