using System;
using System.Collections.Generic;

namespace NoskheAPI_Beta.Models
{
    public class Pharmacy
    {
        public int PharmacyId { get; set; }
        public string Name { get; set; }
        public string UPI { get; set; } // code e sabt
        public DateTime RegisterationDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime ProfilePictureUploadDate { get; set; }
        public double AddressLongitude { get; set; }
        public double AddressLatitude { get; set; }
        public string Address { get; set; }
        public bool IsAvailableNow { get; set; } // baraye tashkhise inke on/off hast
        public decimal Credit { get; set; }
        public uint PendingRequests { get; set; } // tedade request ha vaghti bala raft dige payam nadim
        /*
            n Order
            1 Manager
        */
        public List<Order> Orders { get; set; }
        public int ManagerId { get; set; }
        public Manager Manager { get; set; }
        public Account Account { get; set; }
        public Score Score { get; set; }
        public List<Settle> Settles { get; set; }
        public PharmacyToken PharmacyToken { get; set; }
    }
}