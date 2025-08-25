using Core.Dtos;

namespace Core.Interfaces
{
    /// <summary>
    /// رابط پایه برای همه‌ی سرویس‌های ارسال پیامک
    /// </summary>
    public interface ISmsProvider
    {
        /// <summary>
        /// ارسال پیامک ساده به یک گیرنده با خط اختصاصی
        /// </summary>
        Task<SmsSendResult> SendMessageAsync(string phoneNumber, string message);

        /// <summary>
        /// ارسال پیامک ساده به چند گیرنده با خط اختصاصی
        /// </summary>
        Task SendBulkMessageAsync(IEnumerable<string> phoneNumbers, string message);

        /// <summary>
        /// ارسال پیامک با استفاده از کد الگو (Template) به یک گیرنده
        /// </summary>
        Task SendTemplateAsync(string phoneNumber, string message);

        /// <summary>
        /// ارسال پیامک با استفاده از کد الگو (Template) به چند گیرنده
        /// </summary>
        Task SendBulkTemplateAsync(IEnumerable<string> phoneNumbers, string message);

        /// <summary>
        /// ارسال رمز یکبار مصرف (OTP) به یک گیرنده
        /// </summary>
        Task SendOtpAsync(string phoneNumber, string otpCode);
        Task SendOtpAsync(string phoneNumber);

        /// <summary>
        /// ارسال رمز یکبار مصرف (OTP) با استفاده از الگو
        /// </summary>
        Task<SmsSendResult> SendOtpTemplateAsync(string phoneNumber, string otpCode);
    }
}
