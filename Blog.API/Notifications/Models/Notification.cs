using Blog.API.Notifications.Abstraction;

namespace Blog.API.Notifications.Models
{
  public class Notification<T> : INotification
  {
    public NotificationType NotificationType { get; set; }
    public T Payload { get; set; }

  }
}