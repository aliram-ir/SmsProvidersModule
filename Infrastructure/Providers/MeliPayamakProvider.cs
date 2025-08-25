using Core.Dtos;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace Infrastructure.Providers
{
    public class MeliPayamakProvider : ISmsProvider
    {
        private readonly SmsProviderEntry _entry;
        private readonly Uri _baseAddress = new Uri("https://console.melipayamak.com");
        private readonly List<SmsTemplate> _smsTemplates = [];
        private readonly string _templateCode;

        public MeliPayamakProvider(SmsProviderEntry entry, string templateCode)
        {
            _entry = entry ?? throw new ArgumentNullException(nameof(entry));
            _templateCode = templateCode;
            _smsTemplates = GetTemplates();
        }

        private string GetApiKey() => _entry.ApiKey ?? throw new InvalidOperationException("API Key not configured.");

        private string GetDefaultLine()
        {
            var defaultLine = _entry.Lines
                .FirstOrDefault(l =>
                    l.IsActive &&
                    l.IsDefault);
            if (defaultLine is null) return string.Empty;
           
            return defaultLine.LineNumber ?? string.Empty;
        }
        private List<SmsTemplate> GetTemplates()
        {
            return _entry.Templates.ToList() ?? [];
        }

        private async Task<string> PostAsync(string relativeUrl, object payload)
        {
            using var client = new HttpClient { BaseAddress = _baseAddress };
            var result = await client.PostAsJsonAsync(relativeUrl, payload);
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
        }

        // ارسال پیامک ساده به یک گیرنده
        public async Task<SmsSendResult> SendMessageAsync(string phoneNumber, string message)
        {
            try
            {
                var payload = new
                {
                    from = GetDefaultLine(),
                    to = phoneNumber,
                    text = message
                };

                var response = await PostAsync($"api/send/simple/{GetApiKey()}", payload);

                Console.WriteLine($"[MeliPayamak Simple] Response: {response}");

                // اینجا response رو می‌تونی parse هم بکنی اگه فرمتش مشخصه
                return SmsSendResult.SuccessResult(
                    recId: "0",                  // اگه provider کد رهگیری می‌ده اینجا بگذار
                    providerStatus: "Sent",
                    rawResponse: response
                );
            }
            catch (Exception ex)
            {
                return SmsSendResult.FailureResult(
                    errorMessage: ex.Message
                );
            }
        }

        // ارسال پیامک ساده به چند گیرنده
        public async Task SendBulkMessageAsync(IEnumerable<string> phoneNumbers, string message)
        {
            var payload = new
            {
                from = GetDefaultLine(),
                to = string.Join(",", phoneNumbers),
                text = message
            };
            var response = await PostAsync($"api/send/simple/{GetApiKey()}", payload);
            Console.WriteLine($"[MeliPayamak Bulk Simple] Response: {response}");
        }

        // ارسال پیامک با الگو (Template)
        public async Task SendTemplateAsync(string phoneNumber, string message)
        {
            var payload = new
            {
                bodyId = _templateCode,
                to = phoneNumber,
                message = message
            };
            var response = await PostAsync($"api/send/shared/{GetApiKey()}", payload);
            Console.WriteLine($"[MeliPayamak Template] Response: {response}");
        }

        // ارسال گروهی با الگو
        public async Task SendBulkTemplateAsync(IEnumerable<string> phoneNumbers, string message)
        {
            var payload = new
            {
                bodyId = _templateCode,
                to = string.Join(",", phoneNumbers),
                message = message
            };
            var response = await PostAsync($"api/send/shared/{GetApiKey()}", payload);
            Console.WriteLine($"[MeliPayamak Bulk Template] Response: {response}");
        }

        // ارسال OTP به یک گیرنده (با مشخص بودن کد)
        public async Task SendOtpAsync(string phoneNumber, string otpCode)
        {
            // اگر نیاز به ارسال در متن باشد میشه Simple یا Shared استفاده کرد
            var payload = new { to = phoneNumber };
            var response = await PostAsync($"api/send/otp/{GetApiKey()}", payload);
            Console.WriteLine($"[MeliPayamak OTP with Code:{otpCode}] Response: {response}");
        }

        // ارسال OTP به یک گیرنده (بدون کد دستی)
        public async Task SendOtpAsync(string phoneNumber)
        {
            var payload = new { to = phoneNumber };
            var response = await PostAsync($"api/send/otp/{GetApiKey()}", payload);
            Console.WriteLine($"[MeliPayamak OTP] Response: {response}");
        }

        // ارسال OTP با استفاده از الگو
        public async Task<SmsSendResult> SendOtpTemplateAsync(string phoneNumber, string otpCode)
        {
            try
            {
                var body = _smsTemplates
                    .SingleOrDefault(x => x.TemplateType == SmsTemplateType.OTP)
                    ?.TemplateCode;

                var payload = new
                {
                    bodyId = body,
                    to = phoneNumber,
                    args = new[] { otpCode }
                };

                var rawResponse = await PostAsync($"api/send/shared/{GetApiKey()}", payload);

                // فرض: ملی پیامک خروجی JSON داره
                var parsed = JsonSerializer.Deserialize<MeliPayamakResponse>(rawResponse);

                if (parsed != null && parsed.RecId > 0 && string.IsNullOrWhiteSpace(parsed.Status))
                {
                    return SmsSendResult.SuccessResult(parsed.RecId.ToString(), parsed.Status, rawResponse);
                }

                return SmsSendResult.FailureResult("Send failed", parsed?.Status, rawResponse);
            }
            catch (Exception ex)
            {
                return SmsSendResult.FailureResult(ex.Message);
            }
        }
    }
}
