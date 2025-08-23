namespace Core.Entities
{
    public class SmsLineEntry
    {
        public int Id { get; set; } // کلید اصلی
        public int ProviderEntryId { get; set; } // ارتباط با پروایدر
        public string LineNumber { get; set; } = null!; // شماره خط (مثلاً 5000xxx)
        public bool IsActive { get; set; } = true;
        public bool IsDefault { get; set; } = false;

        // Navigation property
        public SmsProviderEntry? ProviderEntry { get; set; }
    }
}
