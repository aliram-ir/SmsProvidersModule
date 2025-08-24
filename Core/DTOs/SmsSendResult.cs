namespace Core.Dtos
{
    /// <summary>
    /// نتیجه استاندارد ارسال پیامک بدون وابستگی به Provider خاص
    /// </summary>
    public class SmsSendResult
    {
        /// <summary>
        /// وضعیت موفقیت
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// آی‌دی بازگشتی از Provider در صورت موفقیت
        /// </summary>
        public string? RecId { get; set; }

        /// <summary>
        /// پیام وضعیت یا خطا که Provider برگردانده
        /// </summary>
        public string? ProviderStatus { get; set; }

        /// <summary>
        /// پیام خطای داخلی یا Exception
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// پاسخ خام برگشتی از Provider (برای ذخیره و Debug)
        /// </summary>
        public string? RawResponse { get; set; }

        public static SmsSendResult SuccessResult(string recId, string? providerStatus = null, string? rawResponse = null)
        {
            return new SmsSendResult { Success = true, RecId = recId, ProviderStatus = providerStatus, RawResponse = rawResponse };
        }

        public static SmsSendResult FailureResult(string? errorMessage, string? providerStatus = null, string? rawResponse = null)
        {
            return new SmsSendResult { Success = false, ErrorMessage = errorMessage, ProviderStatus = providerStatus, RawResponse = rawResponse };
        }
    }
}
