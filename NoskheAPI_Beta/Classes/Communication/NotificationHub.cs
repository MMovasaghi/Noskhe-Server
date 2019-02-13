using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics;
using NoskheAPI_Beta.Models.Minimals.Output;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using NoskheAPI_Beta.Models;
using Microsoft.EntityFrameworkCore;

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
            string name = Context.User.Identity.Name;
            using(var db = new NoskheAPI_Beta.Models.NoskheContext())
            {
                var existingPharmacy = db.Pharmacies.Where(p => p.PharmacyId == int.Parse(name.Substring(1,name.Length - 1))).FirstOrDefault();
                existingPharmacy.IsAvailableNow = false;
                db.SaveChanges();
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, name);
            await base.OnDisconnectedAsync(exception);
        }
    }
}