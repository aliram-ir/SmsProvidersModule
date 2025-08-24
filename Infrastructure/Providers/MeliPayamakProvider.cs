using Core.Dtos;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace Infrastructure.Providers
{
    public class MeliPayamakProvider : ISmsProvider
    {
        private readonly SmsProviderEntry _entry;
        private readonly Uri _baseAddress = new Uri("https://console.melipayamak.com");

        public MeliPayamakProvider(SmsProviderEntry entry)
        {
            _entry = entry ?? throw new ArgumentNullException(nameof(entry));
        }

        private string GetApiKey()
        {
            return _entry.ApiKey ?? throw new InvalidOperationException("API Key not configured for MeliPayamakProvider.");
        }

        private string GetFromLineOrDefault(string? fromLine = null)
        {
            if (!string.IsNullOrWhiteSpace(fromLine))
                return fromLine;

            return _entry.Lines.FirstOrDefault()?.LineNumber
                   ?? throw new InvalidOperationException("No sending line configured for MeliPayamakProvider.");
        }

        private async Task<string> PostAsync(string relativeUrl, object payload)
        {
            using var client = new HttpClient { BaseAddress = _baseAddress };
            var result = await client.PostAsJsonAsync(relativeUrl, payload);
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
        }

        // ارسال پیامک ساده به یک گیرنده
        public async Task<SmsSendResult> SendMessageAsync(string fromLine, string phoneNumber, string message)
        {
            try
            {
                var payload = new
                {
                    from = GetFromLineOrDefault(fromLine),
                    to = phoneNumber,
                    text = message
                };

                var response = await PostAsync($"api/send/simple/{GetApiKey()}", payload);

                Console.WriteLine($"[MeliPayamak Simple] Response: {response}");

                // اینجا response رو می‌تونی parse هم بکنی اگه فرمتش مشخصه
                return SmsSendResult.SuccessResult(
                    recId: null,                  // اگه provider کد رهگیری می‌ده اینجا بگذار
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
        public async Task SendBulkMessageAsync(string fromLine, IEnumerable<string> phoneNumbers, string message)
        {
            var payload = new
            {
                from = GetFromLineOrDefault(fromLine),
                to = string.Join(",", phoneNumbers),
                text = message
            };
            var response = await PostAsync($"api/send/simple/{GetApiKey()}", payload);
            Console.WriteLine($"[MeliPayamak Bulk Simple] Response: {response}");
        }

        // ارسال پیامک با الگو (Template)
        public async Task SendTemplateAsync(string templateCode, string phoneNumber, Dictionary<string, string> parameters)
        {
            if (!int.TryParse(templateCode, out var bodyId))
                throw new ArgumentException("TemplateCode must be numeric BodyId for MeliPayamak.", nameof(templateCode));

            var args = parameters.Values.ToArray();

            var payload = new
            {
                bodyId = bodyId,
                to = phoneNumber,
                args = args
            };
            var response = await PostAsync($"api/send/shared/{GetApiKey()}", payload);
            Console.WriteLine($"[MeliPayamak Template] Response: {response}");
        }

        // ارسال گروهی با الگو
        public async Task SendBulkTemplateAsync(string templateCode, IEnumerable<string> phoneNumbers, Dictionary<string, string> parameters)
        {
            if (!int.TryParse(templateCode, out var bodyId))
                throw new ArgumentException("TemplateCode must be numeric BodyId for MeliPayamak.", nameof(templateCode));

            var args = parameters.Values.ToArray();

            var payload = new
            {
                bodyId = bodyId,
                to = string.Join(",", phoneNumbers),
                args = args
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
        public async Task<SmsSendResult> SendOtpTemplateAsync(string templateCode, string phoneNumber, string otpCode)
        {
            try
            {
                if (!int.TryParse(templateCode, out var bodyId))
                    return SmsSendResult.FailureResult("TemplateCode must be numeric");

                var payload = new
                {
                    bodyId = bodyId,
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
