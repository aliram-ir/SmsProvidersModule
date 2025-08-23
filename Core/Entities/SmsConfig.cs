namespace Core.Entities
{
    public class SmsConfig
    {
        public int Id { get; set; }

        /// <summary>
        /// تغییر سامانه در صورت در دسترس نبودن به صورت خودکار
        /// </summary>
        public bool EnableAutoFailoverProvider { get; set; }

        /// <summary>
        /// تغییر خط در صورت ایراد در اپراتور به صورت خودکار
        /// </summary>
        public bool EnableAutoFailoverLineNumber { get; set; }
    }
}
