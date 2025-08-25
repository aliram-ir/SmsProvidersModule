using Core.Entities;

namespace Core.Interfaces
{
    public interface ISmsProviderFactory
    {
        ISmsProvider Create(SmsProviderEntry entry, string templateCode);
    }
}
