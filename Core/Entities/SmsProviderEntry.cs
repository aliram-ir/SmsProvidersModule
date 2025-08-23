using Core.Enums;

namespace Core.Entities
{
    public class SmsProviderEntry
    {
        public int Id { get; set; }
        public SmsProviderType ProviderType { get; set; }
        public string? ApiKey { get; set; } = null;
        public string? Token { get; set; } = null;
        public string? Username { get; set; } = null;
        public string? Password { get; set; } = null;
        public bool IsActive { get; set; } = true;
        public bool IsDefault { get; set; } = false;

        public ICollection<SmsLineEntry> Lines { get; set; } = [];
        public ICollection<SmsTemplate> Templates { get; set; } = [];
    }
}
