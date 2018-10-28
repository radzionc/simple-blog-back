using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Blog.API.Notifications
{
    [Authorize]
    public class NotificationsHub : Hub { }
}