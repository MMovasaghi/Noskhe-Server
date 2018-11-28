using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NoskheAPI_Beta.Classes.Communication
{
    public class NotificationHub : Hub
    {
        public async Task Initialize(string identifier)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, identifier);
            await Clients.Group(identifier).SendAsync("HandleNotification", new { Url1 = "www.people.com", Url2 = "www.Picture.com", Url3 = "www.p.com" });
        }
        public async Task SendMessage(string identifier, object arg1)
        {
            await Clients.Group(identifier).SendAsync("HandleNotification", arg1);
        }
    }
    public class WithOutNoskheFist
    {
        public int Number { get; set; }
        public string List { get; set; }
    }
}