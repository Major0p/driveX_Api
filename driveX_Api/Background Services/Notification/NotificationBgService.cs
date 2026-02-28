using driveX_Api.CommonClasses;

namespace driveX_Api.Background_Services
{
    public class NotificationBgService : BackgroundService
    {
        private bool IsEnable { get; set; } = true;
        public ILogger<NotificationBgService> _logger;
        public NotificationBgService(ILogger<NotificationBgService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (IsEnable)
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Notification service Started", DateTime.UtcNow);

                    var notification = NotificationFactory.CreateService(Constants.SMS_Service);
                    await notification.Send("hello from sms background service");
                    
                    _logger.LogInformation("Notification sent succesfully",DateTime.UtcNow);
                }
            }
        }

        public void SetServiceActive()
        {
            this.IsEnable = true;
            _logger.LogInformation("Notification service Activated",DateTime.UtcNow);
        }

        public void SetServiceInactive()
        {
            this.IsEnable = false;
            _logger.LogInformation("Notification service Inactivated",DateTime.UtcNow);
        }
    }
}
