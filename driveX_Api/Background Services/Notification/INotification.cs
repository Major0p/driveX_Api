namespace driveX_Api.Background_Services.Notification
{
    public interface INotification
    {
        public Task<bool> Send(string msg);
    }
}
