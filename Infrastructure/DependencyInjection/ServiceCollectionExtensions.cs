using Core.Interfaces;
using Infrastructure.Cache;
using Infrastructure.Data;
using Infrastructure.Persistence;
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

            // اینجا Interface به Implementation مپ شده
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddHttpClient("MeliPayamak");

            services.AddScoped<ISmsProviderFactory, SmsProviderFactory>();
            services.AddScoped<ISmsProviderSelector, SmsProviderSelector>();
            services.AddScoped<ISmsService, SmsService>();

            return services;
        }

    }
}
