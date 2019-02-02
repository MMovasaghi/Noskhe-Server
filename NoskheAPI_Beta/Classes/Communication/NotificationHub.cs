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
        public override async Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            await Groups.AddToGroupAsync(Context.ConnectionId, name);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Customers");
            await base.OnDisconnectedAsync(exception);
        }
        // Pharmacy SignalR
        public async Task P_PharmacyReception(int pharmacyId, object prescriptionDetails)
        {
            // Joziyate kholase noskhe va moshtari baraye darukhane ersal mishavad
            string name = Context.User.Identity.Name;
            // await Clients.Group(name).SendAsync("", "");
        }
        // Customer SignalR
        public async Task C_PharmacyInquiry(int customerId, string pharmacyName, bool finalized)
        {
            // Ersale darukhane dar halate estelam be client (movaghati)
            string name = Context.User.Identity.Name;
            // await Clients.Group(name).SendAsync("", "");
        }
        public async Task C_PaymentDetail(int customerId)
        {
            // Ersale darukhane dar halate tayyed shode be client (ghat'ii)
            string name = Context.User.Identity.Name;
            // await Clients.Group(name).SendAsync("", "");
        }
    }
}