using Core.Enums;

namespace Core.Entities
{
    public class SmsTemplate
    {
        public int Id { get; set; }

        public int ProviderEntryId { get; set; } // ارتباط با پروایدر
        // نوع قالب از enum ثابت
        public SmsTemplateType TemplateType { get; set; }

        // متن قالب
        public string TemplateBody { get; set; } = null!; // کد الگو
        // Navigation
        public SmsProviderEntry? ProviderEntry { get; set; }
    }
}
