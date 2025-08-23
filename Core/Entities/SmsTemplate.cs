using Core.Enums;

namespace Core.Entities
{
    public class SmsTemplate
    {
        public int Id { get; set; }

        public int ProviderEntryId { get; set; } // ارتباط با پروایدر

        // نوع قالب از enum ثابت
        public SmsTemplateType TemplateType { get; set; }

        // کد الگو (شناسه سمت سامانه پیامکی)
        public string TemplateCode { get; set; } = null!;

        // متن قالب
        public string TemplateBody { get; set; } = null!;

        // Navigation
        public SmsProviderEntry? ProviderEntry { get; set; }
    }
}
