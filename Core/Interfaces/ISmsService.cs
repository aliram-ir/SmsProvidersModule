using Core.Dtos;

namespace Core.Interfaces
{
    /// <summary>
    /// سرویس سطح بالای ارسال پیامک که از Provider انتخاب‌شده استفاده می‌کند
    /// </summary>
    public interface ISmsService
    {
        // ساده
        /// <summary>
        /// ارسال ساده
        /// </summary>
        /// <param name="fromLine"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<SmsSendResult> SendMessageAsync(string phoneNumber, string message);

        /// <summary>
        /// ارسال ساده کلی
        /// </summary>
        /// <param name="fromLine"></param>
        /// <param name="phoneNumbers"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendBulkMessageAsync(IEnumerable<string> phoneNumbers, string message);

        // الگو
        /// <summary>
        /// ارسال با الگو
        /// </summary>
        /// <param name="templateCode"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task SendTemplateAsync(string phoneNumber, Dictionary<string, string> parameters);
       
        /// <summary>
        /// ارسال کلی با الگو
        /// </summary>
        /// <param name="templateCode"></param>
        /// <param name="phoneNumbers"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task SendBulkTemplateAsync(IEnumerable<string> phoneNumbers, Dictionary<string, string> parameters);

        // OTP
        /// <summary>
        /// ارسال کد تایید
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="otpCode"></param>
        /// <returns></returns>
        Task SendOtpAsync(string phoneNumber, string otpCode);

        /// <summary>
        /// ارسال کد تایید
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task SendOtpAsync(string phoneNumber); // تولید توسط خود Provider

        // OTP الگو
        /// <summary>
        /// ارسال کد تایید
        /// کد تایید در کلاینت ساخته و به این متد ارسال می‌شود
        /// </summary>
        /// <param name="templateCode"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="otpCode"></param>
        /// <returns></returns>
        Task<SmsSendResult> SendOtpTemplateAsync(string phoneNumber, string otpCode);
    }
}
