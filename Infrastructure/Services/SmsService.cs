using Core.Interfaces;

namespace Infrastructure.Services
{
    public class SmsService : ISmsService
    {
        private readonly ISmsProviderSelector _providerSelector;

        public SmsService(ISmsProviderSelector providerSelector)
        {
            _providerSelector = providerSelector;
        }

        public async Task SendSmsAsync(string templateCode, string phoneNumber, Dictionary<string, string> values)
        {
            var provider = await _providerSelector.GetDefaultActiveProviderAsync();
            if (provider == null)
                throw new InvalidOperationException("No active default provider found.");

            // TODO: اتصال واقعی به API سامانه پیامکی
            Console.WriteLine($"[SMS] Sending '{templateCode}' to {phoneNumber} via {provider.ProviderType}");
        }
    }
}
