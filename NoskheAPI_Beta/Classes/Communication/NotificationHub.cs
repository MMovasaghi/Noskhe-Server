using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics;
using NoskheAPI_Beta.Models.Minimals.Output;
using System;
using System.Linq;

namespace NoskheAPI_Beta.Classes.Communication
{
    public class NotificationHub : Hub
    {
        private static NoskheAPI_Beta.Models.NoskheContext db = new NoskheAPI_Beta.Models.NoskheContext();
        /*
         * ***** Template for sending data to pharmacy *****
         * 
         * NoskheForFirstNotificationOnDesktop noskheForFirstNotificationOnDesktop = new NoskheForFirstNotificationOnDesktop()
            {
                Picture_Urls = new List<string>()
                {
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTsgKWaWvMfgSmQjJBETlectexGQ4qM_Yf4eiP44iWKUqBASfGvUA",
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSSGDusBHcHurXuTpvVG2PMu44PjcGAMDjN0QsybEkwwg4Eo1CR",
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRNn35JV-qkvadDzEBizt0v1jYNxbapM6evnbKPUjzlhrGCcq2_",
                },
                Cosmetics = new List<CosmeticOutput>()
                {
                    new CosmeticOutput()
                    {
                        Name = "Lipstick",
                        Number = 3,
                        Price = 100000,
                        CosmeticId = 74,
                        ProductPictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSUFevN_-QwofXToQIEvhGXZ595uE1D23T_jDe3s_s92nP9fg6y",
                    }

                },
                Medicions = new List<MedicineOutput>()
                {
                    new MedicineOutput()
                    {
                        Name = "Asetaminofen",
                        Number = 10,
                        Price = 200000,
                        MedicineId = 186,
                        ProductPictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRg6DW40E6_NSu4FAj0gh8JPKAl-81fiOKQHcbR6QjSuSeH_9zV",
                    },
                    new MedicineOutput()
                    {
                        Name = "Aspirin",
                        Number = 7,
                        Price = 600000,
                        MedicineId = 78,
                        ProductPictureUrl = "https://cdn1.medicalnewstoday.com/content/images/articles/301/301766/bottle-of-aspirin.jpg",
                    },
                },
                Customer = new CustomerOutput()
                {
                    FirstName = "محمدحسین",
                    LastName = "موثقی نیا",
                    Gender = Models.Gender.Male,
                    Phone = "09122184357",
                    ProfilePictureUrl = "https://static.evand.net/images/description/original/26bab6a266285f35db954f9485b0443c.jpg?x-oss-process=image/resize,h_200,w_200"
                },
            };
         */
        public async Task Initialize(int dest, string requestToken)
        {
            if(dest == 0) // customer
            {
                // check if the token is valid
                switch (ValidateTokenHandler(0, requestToken))
                {
                    case 1: // matched user
                        // add user to to group
                        var name = GetUniqueId(0, requestToken).ToString();
                        await Groups.AddToGroupAsync(Context.ConnectionId, name);
                        // tell him/her
                        await Clients.Group(name).SendAsync("RecieveStatus", 1, name); // response: 1
                        break;
                    // rahe hal : ghablesh ye bar check konim dasti -> checkToken() for pharmacy and customer
                    case 0: // db failure
                        // do not add user to to group
                        await Clients.Caller.SendAsync("RecieveStatus", 0, null);
                        break;
                    case -1: // wrong pattern
                        // do not add user to to group
                        await Clients.Caller.SendAsync("RecieveStatus", -1, null);
                        break;
                    case -2: // not matched or banned
                        // do not add user to to group
                        await Clients.Caller.SendAsync("RecieveStatus", -2, null);
                        break;
                    case -3: // expired
                        // do not add user to to group
                        await Clients.Caller.SendAsync("RecieveStatus", -3, null);
                        break;
                    case -4: // duplication occured
                        // do not add user to to group
                        await Clients.Caller.SendAsync("RecieveStatus", -4, null);
                        break;
                    default:
                        break;
                }
            }
            else if(dest == 1) // pharmacy
            {
                // check if the token is valid
                switch (ValidateTokenHandler(1, requestToken))
                {
                    case 1: // matched user
                        // add user to to group
                        var name = GetUniqueId(1, requestToken).ToString();
                        await Groups.AddToGroupAsync(Context.ConnectionId, name);
                        // tell him/her
                        await Clients.Group(name).SendAsync("RecieveStatus", 1, name); // response: 1
                        break;
                    // rahe hal : ghablesh ye bar check konim dasti -> checkToken() for pharmacy and customer
                    case 0: // db failure
                        // do not add user to to group
                        await Clients.Caller.SendAsync("RecieveStatus", 0, null);
                        break;
                    case -1: // wrong pattern
                        // do not add user to to group
                        await Clients.Caller.SendAsync("RecieveStatus", -1, null);
                        break;
                    case -2: // not matched or banned
                        // do not add user to to group
                        // how to tell him/her?
                        await Clients.Caller.SendAsync("RecieveStatus", -2, null);
                        break;
                    case -3: // expired
                        // do not add user to to group
                        // how to tell him/her?
                        await Clients.Caller.SendAsync("RecieveStatus", -3, null);
                        break;
                    case -4: // duplication occured
                        // do not add user to to group
                        await Clients.Caller.SendAsync("RecieveStatus", -4, null);
                        break;
                    default:
                        break;
                }
            }
            // await Clients.Group(identifier).SendAsync("HandleNotification", "LOGGED_IN_SUCCESSFULLY");
        }
        public async Task SendMessage(string identifier, object arg1)
        {
            NoskheForFirstNotificationOnDesktop noskheForFirstNotificationOnDesktop = new NoskheForFirstNotificationOnDesktop()
            {
                Picture_Urls = new List<string>()
                {
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTsgKWaWvMfgSmQjJBETlectexGQ4qM_Yf4eiP44iWKUqBASfGvUA",
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSSGDusBHcHurXuTpvVG2PMu44PjcGAMDjN0QsybEkwwg4Eo1CR",
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRNn35JV-qkvadDzEBizt0v1jYNxbapM6evnbKPUjzlhrGCcq2_",
                },
                Cosmetics = new List<Cosmetic>()
                {
                    new Cosmetic()
                    {
                        Name = "Lipstick",
                        Number = 3,
                        Price = 100000,
                        CosmeticId = 74,
                        ProductPictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSUFevN_-QwofXToQIEvhGXZ595uE1D23T_jDe3s_s92nP9fg6y",
                    }

                },
                Medicions = new List<Medicine>()
                {
                    new Medicine()
                    {
                        Name = "Asetaminofen",
                        Number = 10,
                        Price = 200000,
                        MedicineId = 186,
                        ProductPictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRg6DW40E6_NSu4FAj0gh8JPKAl-81fiOKQHcbR6QjSuSeH_9zV",
                    },
                    new Medicine()
                    {
                        Name = "Aspirin",
                        Number = 7,
                        Price = 600000,
                        MedicineId = 78,
                        ProductPictureUrl = "https://cdn1.medicalnewstoday.com/content/images/articles/301/301766/bottle-of-aspirin.jpg",
                    },
                },
                Customer = new Customer()
                {
                    FirstName = "محمدحسین",
                    LastName = "موثقی نیا",
                    Gender = Models.Gender.Male,
                    Phone = "09122184357",
                    ProfilePictureUrl = "https://static.evand.net/images/description/original/26bab6a266285f35db954f9485b0443c.jpg?x-oss-process=image/resize,h_200,w_200"
                },
            };
            await Clients.Group(identifier).SendAsync("HandleNotification", noskheForFirstNotificationOnDesktop);
        }
        public async Task SendMessagePHONE(string identifier, object arg1)
        {

        }
        private int ValidateTokenHandler(int dest, string requestToken)
        {
            var token = requestToken.Substring("Bearer ".Length).Trim();
            if(requestToken == null || !requestToken.StartsWith("Bearer")) return -1;
            if(dest == 0)
            {
                try
                {
                    var customer = db.CustomerTokens.Where(ct => ct.Token == token).FirstOrDefault();
                    if(customer == null || customer.IsValid == false) return -2;
                    if(DateTime.UtcNow > customer.ValidTo) return -3;
                    if(customer.IsAvailableInSignalR == true) return -4;
                    customer.IsAvailableInSignalR = true;
                    db.SaveChanges();
                }
                catch
                {
                    return 0;
                }
            }
            else if(dest == 1)
            {
                try
                {
                    var pharmacy = db.PharmacyTokens.Where(ct => ct.Token == token).FirstOrDefault();
                    if(pharmacy == null || pharmacy.IsValid == false) return -2;
                    if(DateTime.UtcNow > pharmacy.ValidTo) return -3;
                    if(pharmacy.IsAvailableInSignalR == true) return -4;
                    pharmacy.IsAvailableInSignalR = true;
                    db.SaveChanges();
                }
                catch 
                {
                    return 0;
                }
            }
            return 1;
        }
        private string GetUniqueId(int identifier, string requestToken)
        {
            var token = requestToken.Substring("Bearer ".Length).Trim();
            if(identifier == 0)
            {
                var customer = db.CustomerTokens.Where(ct => ct.Token == token).FirstOrDefault();
                return "C:" + customer.CustomerId;
            }
            if(identifier == 1)
            {
                var pharmacy = db.PharmacyTokens.Where(ct => ct.Token == token).FirstOrDefault();
                return "P:" + pharmacy.PharmacyId;
            }
            return "Oops :|";
        }

    }
}