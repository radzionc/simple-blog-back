namespace Blog.API.Notifications.Abstraction
{
    public interface INotification
    {
        NotificationType NotificationType { get; set; }
    }
}