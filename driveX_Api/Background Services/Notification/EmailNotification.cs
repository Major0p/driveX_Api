using driveX_Api.Background_Services.Notification;

namespace driveX_Api.Background_Services
{
    public class EmailNotification : INotification
    {
        public async Task<bool> Send(string msg)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            Console.WriteLine(msg);
            return true;
        }
    }
}
