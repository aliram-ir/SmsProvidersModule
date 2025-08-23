using Core.Interfaces;
using Infrastructure.Cache;
using Infrastructure.Data;
using Infrastructure.Persistence;
using Infrastructure.Providers;
using Infrastructure.Repositories;
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
            services.AddSingleton<ICacheKeyTracker, CacheKeyTracker>();

            // فقط GenericRepository<> را ثبت می‌کنیم تا UnitOfWork بتواند resolve کند
            services.AddScoped(typeof(GenericRepository<>));

            // UnitOfWork وظیفه‌ی ساخت CachedGenericRepository<TEntity> را دارد
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // HttpClient for MeliPayamak
            services.AddHttpClient("MeliPayamak");

            // Providers and SMS services
            services.AddScoped<ISmsProvider, MeliPayamakProvider>();
            services.AddScoped<ISmsProviderFactory, SmsProviderFactory>();
            services.AddScoped<ISmsProviderSelector, SmsProviderSelector>();
            services.AddScoped<ISmsService, SmsService>();

            return services;
        }
    }
}
