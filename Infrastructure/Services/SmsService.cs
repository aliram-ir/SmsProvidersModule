using Core.Interfaces;

namespace Infrastructure.Services
{
    public class SmsService : ISmsService
    {
        private readonly ISmsProviderSelector _providerSelector;
        private readonly ISmsProviderFactory _providerFactory;

        public SmsService(ISmsProviderSelector providerSelector, ISmsProviderFactory providerFactory)
        {
            _providerSelector = providerSelector;
            _providerFactory = providerFactory;
        }

        private async Task<ISmsProvider> GetDefaultProviderAsync()
        {
            var entry = await _providerSelector.GetDefaultActiveProviderAsync();
            if (entry == null)
                throw new InvalidOperationException("No active default provider found.");
            return _providerFactory.Create(entry);
        }

        public async Task SendMessageAsync(string fromLine, string phoneNumber, string message)
        {
            var provider = await GetDefaultProviderAsync();
            await provider.SendMessageAsync(fromLine, phoneNumber, message);
        }

        public async Task SendBulkMessageAsync(string fromLine, IEnumerable<string> phoneNumbers, string message)
        {
            var provider = await GetDefaultProviderAsync();
            await provider.SendBulkMessageAsync(fromLine, phoneNumbers, message);
        }

        public async Task SendTemplateAsync(string templateCode, string phoneNumber, Dictionary<string, string> parameters)
        {
            var provider = await GetDefaultProviderAsync();
            await provider.SendTemplateAsync(templateCode, phoneNumber, parameters);
        }

        public async Task SendBulkTemplateAsync(string templateCode, IEnumerable<string> phoneNumbers, Dictionary<string, string> parameters)
        {
            var provider = await GetDefaultProviderAsync();
            await provider.SendBulkTemplateAsync(templateCode, phoneNumbers, parameters);
        }

        public async Task SendOtpAsync(string phoneNumber, string otpCode)
        {
            var provider = await GetDefaultProviderAsync();
            await provider.SendOtpAsync(phoneNumber, otpCode);
        }

        public async Task SendOtpAsync(string phoneNumber)
        {
            var provider = await GetDefaultProviderAsync();
            await provider.SendOtpAsync(phoneNumber);
        }

        public async Task SendOtpTemplateAsync(string templateCode, string phoneNumber, string otpCode)
        {
            var provider = await GetDefaultProviderAsync();
            await provider.SendOtpTemplateAsync(templateCode, phoneNumber, otpCode);
        }
    }
}
