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
        public async Task P_PharmacyReception(int pharmacyId, NoskheForFirstNotificationOnDesktop prescriptionDetails)
        {
            // Joziyate kholase noskhe va moshtari baraye darukhane ersal mishavad
            string name = Context.User.Identity.Name;
            await Clients.Group(name).SendAsync("PharmacyReception", prescriptionDetails);
        }

        // Customer SignalR
        public async Task C_PharmacyInquiry(int customerId, string pharmacyName, string courierName, string phone, bool finalized)
        {
            // Ersale darukhane dar halate estelam be client (movaghati ya ghat'ii)
            string name = Context.User.Identity.Name;
            await Clients.Group(name).SendAsync("PharmacyInquiry", pharmacyName, courierName, phone, finalized);
        }
        public async Task C_PaymentDetail(int customerId)
        {
            string name = Context.User.Identity.Name;
            await Clients.Group(name).SendAsync("PaymentDetail", "url");
        }
        public async Task C_CourierDetail(int customerId, string courierName, string phone)
        {
            string name = Context.User.Identity.Name;
            await Clients.Group(name).SendAsync("PaymentDetail", "url", courierName, phone);
        }
    }
}