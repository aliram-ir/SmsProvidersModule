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
        Task SendMessageAsync(string fromLine, string phoneNumber, string message);

        /// <summary>
        /// ارسال پیامک ساده به چند گیرنده با خط اختصاصی
        /// </summary>
        Task SendBulkMessageAsync(string fromLine, IEnumerable<string> phoneNumbers, string message);

        /// <summary>
        /// ارسال پیامک با استفاده از کد الگو (Template) به یک گیرنده
        /// </summary>
        Task SendTemplateAsync(string templateCode, string phoneNumber, Dictionary<string, string> parameters);

        /// <summary>
        /// ارسال پیامک با استفاده از کد الگو (Template) به چند گیرنده
        /// </summary>
        Task SendBulkTemplateAsync(string templateCode, IEnumerable<string> phoneNumbers, Dictionary<string, string> parameters);

        /// <summary>
        /// ارسال رمز یکبار مصرف (OTP) به یک گیرنده
        /// </summary>
        Task SendOtpAsync(string phoneNumber, string otpCode);
        Task SendOtpAsync(string phoneNumber);

        /// <summary>
        /// ارسال رمز یکبار مصرف (OTP) با استفاده از الگو
        /// </summary>
        Task SendOtpTemplateAsync(string templateCode, string phoneNumber, string otpCode);
    }
}
