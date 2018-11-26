using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NoskheAPI_Beta.Classes.Communication
{
    public class NotificationHub : Hub
    {
        public async Task Initialize(string identifier)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, identifier);
            await Clients.Group(identifier).SendAsync("HandleNotification", new { Name="Server: Hey man! wasssssuuup?!", ID=110 });
        }
        public async Task SendMessage(string identifier, object arg1)
        {
            await Clients.Group(identifier).SendAsync("HandleNotification", arg1);
        }
    }
}