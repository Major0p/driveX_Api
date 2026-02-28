using driveX_Api.Background_Services.Notification;
using driveX_Api.CommonClasses;

namespace driveX_Api.Background_Services
{
    public static class NotificationFactory
    {
        public static INotification CreateService(string type)
        {
            return type switch
            {
                Constants.Email_Service => new EmailNotification(),
                Constants.SMS_Service => new SmsNotification(),
                _ => null
            };
        }
    }
}
