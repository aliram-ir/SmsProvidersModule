namespace Core.Interfaces
{
    /// <summary>
    /// سرویس سطح بالای ارسال پیامک که از Provider انتخاب‌شده استفاده می‌کند
    /// </summary>
    public interface ISmsService
    {
        // ساده
        Task SendMessageAsync(string fromLine, string phoneNumber, string message);
        Task SendBulkMessageAsync(string fromLine, IEnumerable<string> phoneNumbers, string message);

        // الگو
        Task SendTemplateAsync(string templateCode, string phoneNumber, Dictionary<string, string> parameters);
        Task SendBulkTemplateAsync(string templateCode, IEnumerable<string> phoneNumbers, Dictionary<string, string> parameters);

        // OTP
        Task SendOtpAsync(string phoneNumber, string otpCode);
        Task SendOtpAsync(string phoneNumber); // تولید توسط خود Provider

        // OTP الگو
        Task SendOtpTemplateAsync(string templateCode, string phoneNumber, string otpCode);
    }
}
