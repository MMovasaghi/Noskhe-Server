using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics;
using NoskheAPI_Beta.Models.Minimals.Output;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace NoskheAPI_Beta.Classes.Communication
{
    [Authorize]
    public class NotificationHub : Hub
    {
        //public override async Task OnConnectedAsync()
        //{
        //    var s = Context;
        //    await Clients.Caller.SendAsync("ChatFunc1", Context.ConnectionId, "Added");
        //    await Groups.AddToGroupAsync(Context.ConnectionId, "Users");
        //    await base.OnConnectedAsync();
        //}
        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    await Clients.All.SendAsync("ChatFunc1", Context.ConnectionId, "Left");
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Users");
        //    await base.OnDisconnectedAsync(exception);
        //}
        //public async Task Echo()
        //{
        //    //await Clients.Client(Context.ConnectionId).SendAsync("ChatFunc2", "MyEcho");
        //    await Clients.Caller.SendAsync("ChatFunc2", "MyEcho");
        //}
        public override async Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            await Groups.AddToGroupAsync(Context.ConnectionId, name);
            await Clients.Group(name).SendAsync("ChatFunc1", name, "Added");

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("ChatFunc1", Context.ConnectionId, "Left");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Users");
            await base.OnDisconnectedAsync(exception);
        }
        public async Task Echo()
        {
            string name = Context.User.Identity.Name;

            await Clients.Group(name).SendAsync("ChatFunc2", "echo!");
        }
    }
}