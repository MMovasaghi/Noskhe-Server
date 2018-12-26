using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics;
using NoskheAPI_Beta.Models.Minimals.Output;

namespace NoskheAPI_Beta.Classes.Communication
{
    public class NotificationHub : Hub
    {
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
        public async Task Initialize(string identifier)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, identifier);
            await Clients.Group(identifier).SendAsync("HandleNotification", "LOGGED_IN_SUCCESSFULLY");
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
    }

}