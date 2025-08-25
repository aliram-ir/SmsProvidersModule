using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services
{
    public class SmsProviderFactory : ISmsProviderFactory
    {
        private readonly IServiceProvider _provider;
        public SmsProviderFactory(IServiceProvider provider) => _provider = provider;

        public ISmsProvider Create(SmsProviderEntry entry, string templateCode)
        {
            // Example: switch on entry.ProviderType or use reflection
            if (entry.ProviderType == SmsProviderType.MeliPayamak)
                return ActivatorUtilities.CreateInstance<MeliPayamakProvider>(_provider, templateCode);
            throw new NotSupportedException(entry.ProviderType.ToString());
        }
    }
}
