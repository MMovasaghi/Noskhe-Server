using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics;
using NoskheAPI_Beta.Models.Minimals;

namespace NoskheAPI_Beta.Classes.Communication
{
    public class NotificationHub : Hub
    {
        public async Task Initialize(string identifier)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, identifier);
            //List<NoskheAtFirst> A = new List<NoskheAtFirst>()
            //{
            //    new NoskheAtFirst()
            //    {
            //        Picture_Urls = new List<string>()
            //        {
            //            "www.google.com",
            //            "www.pashm.com",
            //        },
            //        WithOutNoskhe_List = new List<WithOutNoskhe>()
            //        {
            //            new WithOutNoskhe()
            //            {
            //                Number = 10,
            //                Name = "Pashm"
            //            },
            //            new WithOutNoskhe()
            //            {
            //                Number = 20,
            //                Name = "Salam"
            //            },
            //        },
            //    },
            //};
            string A = "Pashmam Conecting to server";
            await Clients.Group(identifier).SendAsync("HandleNotification", A);
        }
        public async Task SendMessage(string identifier, object arg1)
        {
            await Clients.Group(identifier).SendAsync("HandleNotification", arg1);
        }
        public async Task SendMessagePhone(string identifier, string Data)
        {
            await Clients.Group(identifier).SendAsync("HandleNotification", Data);
        }
    }

}