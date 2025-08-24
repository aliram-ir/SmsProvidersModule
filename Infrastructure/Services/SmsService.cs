using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class SmsService : ISmsService
    {
        private readonly ISmsProviderSelector _providerSelector;
        private readonly ISmsProviderFactory _providerFactory;
        private readonly IGenericRepository<SmsLineEntry> _lineRepo;
        private readonly IGenericRepository<SmsTemplate> _templateRepo;

        public SmsService(
            ISmsProviderSelector providerSelector,
            ISmsProviderFactory providerFactory,
            IGenericRepository<SmsLineEntry> lineRepo,
            IGenericRepository<SmsTemplate> templateRepo)
        {
            _providerSelector = providerSelector;
            _providerFactory = providerFactory;
            _lineRepo = lineRepo;
            _templateRepo = templateRepo;
        }

        /// <summary>
        /// یافتن Provider و خط پیش‌فرض مربوط به آن به همراه شناسه Provider
        /// </summary>
        private async Task<(ISmsProvider provider, string defaultLine, int providerId)> GetDefaultProviderLineAndIdAsync()
        {
            var providerEntry = await _providerSelector.GetDefaultActiveProviderAsync();
            if (providerEntry == null)
                throw new InvalidOperationException("No active default provider found.");

            var defaultLine = (await _lineRepo.GetAllAsync())
                .FirstOrDefault(l =>
                    l.ProviderEntryId == providerEntry.Id &&
                    l.IsActive &&
                    l.IsDefault);

            if (defaultLine == null)
                throw new InvalidOperationException("No default line found for the default provider.");

            return (_providerFactory.Create(providerEntry), defaultLine.LineNumber, providerEntry.Id);
        }

        // --- ساده ---
        public async Task<SmsSendResult> SendMessageAsync(string phoneNumber, string message)
        {
            var (provider, line, _) = await GetDefaultProviderLineAndIdAsync();
            var result = await provider.SendMessageAsync(line, phoneNumber, message);
            return result ?? SmsSendResult.FailureResult("No response from provider.");
        }

        public async Task SendBulkMessageAsync(IEnumerable<string> phoneNumbers, string message)
        {
            var (provider, line, _) = await GetDefaultProviderLineAndIdAsync();
            await provider.SendBulkMessageAsync(line, phoneNumbers, message);
        }

        // --- الگو ---
        public async Task SendTemplateAsync(string phoneNumber, Dictionary<string, string> parameters)
        {
            var (provider, line, providerId) = await GetDefaultProviderLineAndIdAsync();

            var template = (await _templateRepo.GetAllAsync())
                .FirstOrDefault(t =>
                    t.ProviderEntryId == providerId &&
                    t.TemplateType == SmsTemplateType.Standard);

            if (template == null)
                throw new InvalidOperationException("No standard template found for the default provider.");

            await provider.SendTemplateAsync(template.TemplateCode, phoneNumber, parameters);
        }

        public async Task SendBulkTemplateAsync(IEnumerable<string> phoneNumbers, Dictionary<string, string> parameters)
        {
            var (provider, line, providerId) = await GetDefaultProviderLineAndIdAsync();

            var template = (await _templateRepo.GetAllAsync())
                .FirstOrDefault(t =>
                    t.ProviderEntryId == providerId &&
                    t.TemplateType == SmsTemplateType.Standard);

            if (template == null)
                throw new InvalidOperationException("No standard template found for the default provider.");

            await provider.SendBulkTemplateAsync(template.TemplateCode, phoneNumbers, parameters);
        }

        // --- OTP ---
        public async Task SendOtpAsync(string phoneNumber, string otpCode)
        {
            var (provider, line, _) = await GetDefaultProviderLineAndIdAsync();
            await provider.SendOtpAsync(phoneNumber, otpCode);
        }

        public async Task SendOtpAsync(string phoneNumber)
        {
            var (provider, line, _) = await GetDefaultProviderLineAndIdAsync();
            await provider.SendOtpAsync(phoneNumber);
        }

        // --- OTP الگو ---
        public async Task<SmsSendResult> SendOtpTemplateAsync(string phoneNumber, string otpCode)
        {
            try
            {
                var (provider, line, providerId) = await GetDefaultProviderLineAndIdAsync();

                var template = (await _templateRepo.GetAllAsync())
                    .FirstOrDefault(t =>
                        t.ProviderEntryId == providerId &&
                        t.TemplateType == SmsTemplateType.OTP);

                if (template == null)
                    return SmsSendResult.FailureResult("No OTP template found for the default provider.");

                return await provider.SendOtpTemplateAsync(template.TemplateCode, phoneNumber, otpCode);
            }
            catch (Exception ex)
            {
                return SmsSendResult.FailureResult(ex.Message);
            }
        }

    }
}
