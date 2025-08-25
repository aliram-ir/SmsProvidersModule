using Core.Dtos;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class SmsService : ISmsService
    {
        private readonly ISmsProviderSelector _providerSelector;
        private readonly ISmsProviderFactory _providerFactory;

        public SmsService(
            ISmsProviderSelector providerSelector,
            ISmsProviderFactory providerFactory
           )
        {
            _providerSelector = providerSelector;
            _providerFactory = providerFactory;
        }

        /// <summary>
        /// پیدا کردن سامانه پیشفرض
        /// </summary>
        private async Task<ISmsProvider> GetDefaultProviderAsync()
        {
            var providerEntry = await _providerSelector.GetDefaultActiveProviderAsync();
            if (providerEntry == null)
                throw new InvalidOperationException("No active default provider found.");

            return _providerFactory.Create(providerEntry,string.Empty);
        }

        public async Task<SmsSendResult> SendMessageAsync(string phoneNumber, string message)
        {
            var provider = await GetDefaultProviderAsync();

            try
            {
                await provider.SendMessageAsync(phoneNumber, message);
                return SmsSendResult.SuccessResult();
            }
            catch (Exception ex)
            {
                return SmsSendResult.FailureResult(ex.Message);
            }
        }

        public async Task SendBulkMessageAsync(IEnumerable<string> phoneNumbers, string message)
        {
            var provider = await GetDefaultProviderAsync();

            await provider.SendBulkMessageAsync(phoneNumbers, message);
        }

        public async Task SendTemplateAsync(string phoneNumber, string message)
        {
            var provider = await GetDefaultProviderAsync();

            // فرض بر این است که templateCode و پارامترها از جای دیگری تأمین می‌شود
            var parameters = new Dictionary<string, string> { { "msg", message } };
            await provider.SendTemplateAsync(phoneNumber, message);
        }

        public async Task SendBulkTemplateAsync(IEnumerable<string> phoneNumbers, string message)
        {
            var provider = await GetDefaultProviderAsync();

            var parameters = new Dictionary<string, string> { { "msg", message } };
            await provider.SendBulkTemplateAsync(phoneNumbers, message);
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

        public async Task<SmsSendResult> SendOtpTemplateAsync(string phoneNumber, string otpCode)
        {
            var provider = await GetDefaultProviderAsync();
            try
            {
                await provider.SendOtpTemplateAsync(phoneNumber, otpCode);
                return SmsSendResult.SuccessResult();
            }
            catch (Exception ex)
            {
                return SmsSendResult.FailureResult(ex.Message);
            }
        }

    }
}
