using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum SmsTemplateType
    {
        None = 0,

        /// <summary>
        /// خوشامدگویی به کاربر جدید
        /// </summary>
        Welcome = 1,

        /// <summary>
        /// ریست رمز عبور
        /// </summary>
        PasswordReset = 2,

        /// <summary>
        /// کد تایید یکبار مصرف (OTP)
        /// </summary>
        OTP = 3,

        /// <summary>
        /// اعلانات و اطلاعیه‌ها
        /// </summary>
        Promotion = 4,

        /// <summary>
        /// اعلانات و نوتیفیکیشن‌ها
        /// </summary>
        Notification = 5,

        /// <summary>
        /// صادر کردن فاکتور یا صورتحساب
        /// </summary>
        Invoice = 6,

        /// <summary>
        /// حمل و نقل و پیگیری سفارشات
        /// </summary>
        ShipmentTracking = 7
    }
}
