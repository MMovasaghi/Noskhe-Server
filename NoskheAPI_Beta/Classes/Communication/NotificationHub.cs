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
<<<<<<< HEAD
=======
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Customers");
            await base.OnDisconnectedAsync(exception);
        }

        // Pharmacy SignalR
        public async Task P_PharmacyReception(int pharmacyId, NoskheForFirstNotificationOnDesktop prescriptionDetails)
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
                    },
                    new Cosmetic()
                    {
                        Name = "Remel",
                        Number = 3,
                        Price = 200000,
                        CosmeticId = 89,
                        ProductPictureUrl = "https://assets.fishersci.com/TFS-Assets/MBD/product-images/F103904~p.eps-650.jpg",
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
                Notation = new Models.Notation()
                {
                    Description = "پشمای من فر خوردن",
                }

            };
            // Joziyate kholase noskhe va moshtari baraye darukhane ersal mishavad
            string name = Context.User.Identity.Name;
            await Clients.Group(name).SendAsync("PharmacyReception", noskheForFirstNotificationOnDesktop);
        }

        // Customer SignalR
        public async Task C_PharmacyInquiry(int customerId, string pharmacyName, string courierName, string phone, bool finalized)
        {
            // Ersale darukhane dar halate estelam be client (movaghati ya ghat'ii)
>>>>>>> 55f91a942d416062f9631ab4ecd0c470fac306f7
            string name = Context.User.Identity.Name;
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, name);
            await base.OnDisconnectedAsync(exception);
        }
    }
}