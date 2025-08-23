using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Providers;

namespace Infrastructure.Services
{
    public class SmsProviderFactory : ISmsProviderFactory
    {
        public ISmsProvider Create(SmsProviderEntry entry)
        {
            return entry.ProviderType switch
            {
                SmsProviderType.MeliPayamak => new MeliPayamakProvider(entry),
                _ => throw new NotSupportedException($"Provider type '{entry.ProviderType}' is not supported."),
            };
        }
    }
}
