using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Classes.Communication
{
    /*
     * Template For Sending Noskhe To Pharmacy : 
     * 
     *  {
     *      Url1 = "www.Sample.com",
     *      Url2 = "www.Sample.com",
     *      Url3 = "www.Sample.com",
     *      
     *  }
     */
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
    public class NoskheAtFirst
    {
        public List<string> Picture_Urls { set; get; }
        public List<WithOutNoskhe> WithOutNoskhe_List { set; get; }
    }
    public class WithOutNoskhe
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }

}